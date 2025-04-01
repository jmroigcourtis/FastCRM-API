using CRM.Data;
using CRM.Data.Entities;
using CRM.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using CRM.Services.Implementations;
using Microsoft.IdentityModel.Tokens;


namespace CRM.Controllers;
[ApiController]
[Route("[controller]")]
public class TasksFollowUpController:ControllerBase
{
    private readonly CrmdbContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly MailService _mailService;

    public TasksFollowUpController (CrmdbContext context, ILogger<UserController> logger, MailService mailService)
    {
        _context = context;
        _logger = logger;
        _mailService = mailService;
    }

    [HttpPost("CreateTask")]
    public async Task<ActionResult<TaskFollowUpDto>> CreateTask(TaskFollowUpDto reqBody)
    {
        if (reqBody.TaskName.ToString().IsNullOrEmpty())
        {
            _logger.LogError("A task name is required");
            return BadRequest("Task name empty or null");
        }

        var newTask = new TasksFollowUpEntity
        {
            TaskName = reqBody.TaskName,
            ClientId = reqBody.ClientId,
            StartDate = reqBody.StartDate,
            EndDate = reqBody.EndDate,
            Priority = reqBody.Priority,
            Status = reqBody.Status,
            AssignedTo = reqBody.AssignedTo,
        };
        
        _logger.LogInformation("Creating new task");
        await _context.Set<TasksFollowUpEntity>().AddAsync(newTask);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created new task");

        var createdTask = await _context.Set<TasksFollowUpEntity>().FindAsync(newTask.Id);
        
        
        var message = $"New task created \n" +
                      $"ID: {createdTask.Id}\n" +
                      $"Task name: {createdTask.TaskName}\n" +
                      $"Task start date: {createdTask.StartDate}\n" +
                      $"Task end date: {createdTask.EndDate}\n" +
                      $"Task priority: {createdTask.Priority}\n" +
                      $"Task status: {createdTask.Status}\n" +
                      $"Task assigned to: {createdTask.AssignedTo}\n";

        var subject = $"Task {createdTask.Id} created and assigned to your group";
        
        //_mailService.SendRegisterMail()
        
        return Ok($"Task #{newTask.TaskName} was created and assigned to {newTask.AssignedTo}");
    }

    [HttpDelete("DeleteTask/{id}")]
    public async Task<ActionResult<TaskFollowUpDto>> DeleteTask(int id)
    {
        var task = await _context.Set<TasksFollowUpEntity>().FindAsync(id);
        if (task == null)
        {
            _logger.LogError($"Task with id {id} not found");
            return NotFound();
        }
        _context.Set<TasksFollowUpEntity>().Remove(task);
        await _context.SaveChangesAsync();
        return Ok($"Task: {task.TaskName} successfully deleted");
    }
    
}