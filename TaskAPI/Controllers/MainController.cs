using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskAPI.Data;
using TaskAPI.Dtos.User;
using TaskAPI.Model;

namespace TaskAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class MainController:Controller
    {
        private readonly GetDbContext _db;

        public MainController(GetDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            return Ok(_db.Employee);
        }
        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetById(int empId)
        {
            var res = await _db.Employee.FirstOrDefaultAsync(s => s.EmpId == empId);
            return Ok(res);
        }
   
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> AddNew(Employee employee)
        { 
             var response = new ServiceResponse<string>();

            try
            {
                if (employee.EmpCode == null)
                {
                    response.Success = false;
                    response.Data = null;
                    response.Message = "Invalid Employee Code";
                }
                else if (employee.EmpName is null)
                {
                    response.Success = false;
                    response.Data = null;
                    response.Message = "Invalid Employee Name";

                }
                else if (employee.Salary == null)
                {
                    response.Success = false;
                    response.Data = null;
                    response.Message = ("Invalid Employee Salary");
                }
                else
                {
                    _db.Employee.Add(employee);
                   await _db.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                response.Error.Add(ex.Message);
            }
            return Ok(response);
        }
 

        [HttpPut("id")]
        public async Task<ActionResult<UpdateDTO>> Update(int empId, UpdateDTO employee)
        //{
        //    Employee model = new()
        //    {
        //    EmpCode = employee.EmpCode,
        //    EmpName = employee.EmpName,
        //    DeptId = employee.DeptId,
        //    Salary = employee.Salary
        //};
        //    _db.Employee.Update(model);
        //    await _db.SaveChangesAsync();
        //    return NoContent();
        //}
        {
            var res = _db.Employee.FirstOrDefault(x => x.EmpId == empId);
            res.EmpCode= employee.EmpCode;
            res.EmpName= employee.EmpName;
            res.DeptId=employee.DeptId;
            res.Salary= employee.Salary;    
            _db.SaveChanges();
            return Ok(res);

        }

        [HttpDelete("id")]
        public ActionResult Del(int Empid)
        {
            var res = _db.Employee.FirstOrDefault(x => x.EmpId == Empid);
            _db.Employee.Remove(res);
            _db.SaveChanges();
            return NoContent();
        }


        


    }
}
