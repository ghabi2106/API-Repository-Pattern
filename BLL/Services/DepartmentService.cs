using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        Task<Department> CreateAsync(DepartmentCreateRequestViewModel request);
        Task<List<Department>> GetAllAsync();
        Task<Department> FindAsync(string code);
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);
        Task<Department> UpdateAsync(string code, Department department);
        Task<Department> DeleteAsync(string code);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Department> CreateAsync(DepartmentCreateRequestViewModel request)
        {
            Department department = new Department()
            {
                Code = request.Code,
                Name = request.Name
            };
            await _unitOfWork.DepartmentRepository.CreateAsync(department);
            if (await _unitOfWork.DepartmentRepository.SaveAsync())
                return department;
            throw new ApplicationValidationException("Some problem with delete data");
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _unitOfWork.DepartmentRepository.GetList();
        }

        public async Task<Department> FindAsync(string code)
        {
            return await _unitOfWork.DepartmentRepository.FindAsync(x => x.Code == code) ??
                throw new ApplicationValidationException("Department not found");
        }

        public async Task<bool> IsCodeExists(string code)
        {
            return await _unitOfWork.DepartmentRepository.Exists(x => x.Code == code);
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _unitOfWork.DepartmentRepository.Exists(x => x.Name == name);
        }

        public async Task<bool> IsIdExists(int id)
        {
            return await _unitOfWork.DepartmentRepository.Exists(x => x.Id == id);
        }
        public async Task<Department> DeleteAsync(string code)
        {
            Department department = await _unitOfWork.DepartmentRepository.FindAsync(x => x.Code == code);
            if (department == null)
                throw new ApplicationValidationException("Department not found");
            _unitOfWork.DepartmentRepository.Delete(department);
            if (await _unitOfWork.DepartmentRepository.SaveAsync())
                return department;
            throw new ApplicationValidationException("Some problem with delete data");
        }

        public async Task<Department> UpdateAsync(string code, Department department)
        {
            Department oldDepartment = await _unitOfWork.DepartmentRepository.FindAsync(x => x.Code == code);
            if (oldDepartment == null)
                throw new ApplicationValidationException("Department not found");
            if (!string.IsNullOrWhiteSpace(department.Code))
            {
                if(await _unitOfWork.DepartmentRepository.Exists(x => x.Code == department.Code) && department.Code!=code)
                    throw new ApplicationValidationException("The code already exists");
                oldDepartment.Code = department.Code;
            }
            if (!string.IsNullOrWhiteSpace(department.Code))
            {
                if (await _unitOfWork.DepartmentRepository.Exists(x => x.Name == department.Name) 
                    && department.Name != oldDepartment.Name)
                    throw new ApplicationValidationException("The name already exists");
                oldDepartment.Name = department.Name;
            }
            _unitOfWork.DepartmentRepository.Update(oldDepartment);
            if (await _unitOfWork.DepartmentRepository.SaveAsync())
                return oldDepartment;
            throw new ApplicationValidationException("Some problem with delete data");
        }
    }
}
