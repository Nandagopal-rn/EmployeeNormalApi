using ClassLibraryEmployee.Employee;
using DataLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ClassLibraryEmployee.Controllers
{
    [Route("api/employeeees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNewEmployee()
        {
            try
            {
                var employes = new EmployeeRepository();

                var getEmployee = employes.GetDetails();

                return Ok(getEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEmploy([FromRoute] int id)
        {
            try
            {
                IdValidate(id);
                var empRepo = new EmployeeRepository();
                var employ = empRepo.ViewDetails(id);

                return Ok(employ);
            }
            catch(ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        
        public IActionResult InsertDetail([FromBody] Employeeee employ)
        {
            try
            {
                ValidateEmp(employ);
                var insertEmp = new EmployeeRepository();
                var result = insertEmp.InsertDetails(employ);

                return Ok(result);
            }
            catch(ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateEmp([FromRoute] int id,[FromBody] Employeeee employ)
        {
            try
            {
                ValidateEmp(employ);
                bool emptyData = EmptyData(id);

                var empRepo = new EmployeeRepository();

                if (emptyData)
                {
                    employ.Id = id;
                    empRepo.UpdateDetails(employ);
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    empRepo.InsertDetails(employ);
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]

        public IActionResult DelEmp([FromRoute] int id)
        {
            IdValidate(id);
            var delEmp = new EmployeeRepository();
            delEmp.DeleteDetails(id);

            return Ok();
        }


        private void IdValidate(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Enter a valid id");
            }
        }
        private void ValidateEmp(Employeeee employ)
        {
            if (employ.Name == null)
            {
                throw new ArgumentNullException("Enter a valid Name");
            }
            if (employ.Age <= 0)
            {
                throw new ArgumentException("Enter a valid age");
            }
        }
        private bool EmptyData(int id)
        {
            var emptyDetails = new EmployeeRepository();
            var myData = emptyDetails.ViewDetails(id);
            bool isDataExist = false;
            if (myData != null)
            {
                isDataExist = true;
            }
            return isDataExist;
        }
    }
}
