using AutoMapper;
using Contracts;
using Entities.Models;
using LuggageShare.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuggageShare.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper; 
        public OwnerController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper; 
        }  

        [HttpGet]
        public IActionResult GetAllOwners()
        {


            try
            {
                var owners = _repository.Owner.GetAllOwners();
                if(owners == null)
                {
                    _logger.LogError("Internal server error"); 
                    return NotFound(); 
                }
                _logger.LogInfo("Return all the owners from database");

                var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners); 
                return Ok(ownersResult); 
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwner action: {ex.Message}");
                return StatusCode(500, "INternal server error"); 
            }
        }

        //[HttpGet("{id}")]
        [HttpGet("{id}", Name ="OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in the database ");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner id: {id}");
                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "INternal server Error"); 
            }
        }


        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in the database ");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with detail for id: {id}");
                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "INternal server Error");
            }
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody]OwnerForCreationDto owner)
        {
            try
            {
                if(owner == null)
                {
                    _logger.LogError("Owner object should not be null");
                    return BadRequest("Invalid model object"); 
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Object sent to the client");
                    return BadRequest("Invalid Model Object"); 
                }

                var ownerEntity = _mapper.Map<Owner>(owner);
                _repository.Owner.CreateOwner(ownerEntity);
                _repository.Save();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtAction("OwnerById", new { id = createdOwner.Id }, createdOwner); 
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal Server error"); 
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOwner( Guid id, [FromBody] OwnerForUpdateDto owner)
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError("Owner object sent from ckient is null");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Object sent from  client");
                    return BadRequest("Invalid Model Object");
                }
                var ownerEntity = _repository.Owner.GetOwnerById(id); 
                if(ownerEntity == null)
                {
                    _logger.LogError($"Owner id with : {id}, has not been found");
                    return NotFound(); 
                }

                _mapper.Map(owner, ownerEntity); 
                _repository.Owner.UpdateOwner(ownerEntity);
                _repository.Save();


                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
                return StatusCode(500, "Internal Server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteResult(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, has not been found");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal Server error");
            }       
            
        }
    }
}
