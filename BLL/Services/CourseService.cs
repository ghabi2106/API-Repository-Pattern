﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Utility;
using Utility.Exceptions;
using Utility.Helpers;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> CreateAsync(CourseCreateRequestViewModel request);
        Task<List<Course>> GetAllAsync();
        Task<Course> UpdateAsync(string code, Course department);
        Task<Course> DeleteAsync(string code);
        Task<Course> FindAsync(string code);

        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);
        Task Testing(RequestMaker loginUser);

    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IConfiguration _configuration;
        private readonly string _server;
        private readonly string _accesskey;
        private readonly string _secretKey;
        private readonly string _bucketName;


        public CourseService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _server = configuration.GetValue<string>("MediaServer:ImageServer");
            _accesskey = configuration.GetValue<string>("MediaServer:AccessKey");
            _secretKey = configuration.GetValue<string>("MediaServer:SecretKey");
            _bucketName = configuration.GetValue<string>("MediaServer:BucketName");

        }

        public async Task<Course> CreateAsync(CourseCreateRequestViewModel request)
        {
            var course = new Course();
            course.Code = request.Code;
            course.Name = request.Name;
            course.Credit = request.Credit;
            course.ImageUrl = await ForImageUpload(request.CourseImage); ;
            await _unitOfWork.CourseRepository.CreateAsync(course);

            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("course insert has some problem");
        }

        private async Task<string> ForImageUpload(IFormFile file)
        {
            var client = new MinioClient(_server, _accesskey, _secretKey);
            await SetupBucket(client, _bucketName);
            var extension = Path.GetExtension((file.FileName)) ?? ".png";
            var fileName = Guid.NewGuid().ToString() + extension;
            var imagePath = _configuration.GetValue<string>("MediaServer:LocalImageStorage");
            var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, fileName).ToLower();
            await using var bits = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(bits);
            bits.Close();

            await client.PutObjectAsync(_bucketName, fileName, path, "image/jpeg");
            File.Delete(path);
            return fileName;

            //var extension = Path.GetExtension((file.FileName)) ?? ".png";
            //var fileName = Guid.NewGuid().ToString() + extension;
            //var imagePath = _configuration.GetValue<string>("MediaServer:LocalImageStorage");
            //var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, fileName).ToLower();
            //await using var bits = new FileStream(path, FileMode.Create);
            //await file.CopyToAsync(bits);
            //bits.Close();
            //return fileName;
        }
        private async Task SetupBucket(MinioClient client, string bucket)
        {
            var found = await client.BucketExistsAsync(bucket);
            if (!found)
            {
                await client.MakeBucketAsync(bucket);
            }
        }

        public async Task<List<Course>> GetAllAsync()
        {
            var allCourse = await _unitOfWork.CourseRepository.GetList();
            var latest = allCourse.Select(c =>
            {
                c.ImageUrl = _configuration.GetValue<string>("MediaServer:ImageAccessUrl") + c.ImageUrl;
                return c;
            }).ToList();
            return latest;
        }

        public async Task<Course> UpdateAsync(string code, Course aCourse)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            if (!string.IsNullOrWhiteSpace(aCourse.Code))
            {
                var existsAlreadyCode = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);
                if (existsAlreadyCode != null)
                {
                    throw new ApplicationValidationException("your updated code already present in our system");
                }

                course.Code = aCourse.Code;
            }

            if (!string.IsNullOrWhiteSpace(aCourse.Name))
            {
                var existsAlreadyCode = await _unitOfWork.CourseRepository.FindAsync(x => x.Name == aCourse.Name);
                if (existsAlreadyCode != null)
                {
                    throw new ApplicationValidationException("your updated name already present in our system");
                }

                course.Name = aCourse.Name;
            }

            _unitOfWork.CourseRepository.Update(course);
            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("in update have some problem");
        }

        public async Task<Course> DeleteAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            _unitOfWork.CourseRepository.Delete(course);
            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("some problem for delete data");
        }

        public async Task<Course> FindAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            course.ImageUrl = _configuration.GetValue<string>("MediaServer:ImageAccessUrl") + course.ImageUrl;
            return course;
        }

        public async Task<bool> IsCodeExists(string code)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Code == code);
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Name == name);
        }

        public async Task<bool> IsIdExists(int id)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Id == id);
        }

        public Task Testing(RequestMaker loginUser)
        {

            var userId = loginUser.Principal.GetUserId();
            var userName = loginUser.Principal.GetUserName();
            var userRole = loginUser.Principal.GetUserRole();
            throw new NotImplementedException();
        }
    }
}
