using ClassesOrganizationSystem.Application.Models.LessonDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
        {
            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            var subjectDto = SubjectDto.MapFromSubject(subject);

            return Ok(subjectDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> ListSubjects(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            var subjects = await _unitOfWork.ScheduleRepository
                .ListSubjectsAsync(query, pageNum, pageSize);

            var subjectDtos = new List<SubjectDto>();

            foreach (var subject in subjects)
            {
                var subjectDto = SubjectDto.MapFromSubject(subject);

                subjectDtos.Add(subjectDto);
            }

            return Ok(subjectDtos);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(
            CreateSubjectDto createSubjectDto)
        {
            var subject = createSubjectDto.MapToSubject();

            _unitOfWork.ScheduleRepository.AddSubject(subject);

            await _unitOfWork.SaveChangesAsync();

            var subjectDto = SubjectDto.MapFromSubject(subject);

            return CreatedAtAction(nameof(GetSubjectById),
                new
                {
                    subject.Id
                },
                subjectDto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateSubject(
            int id, [FromBody] JsonPatchDocument patchDocument)
        {
            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(subject);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveSubject(int id)
        {
            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            _unitOfWork.ScheduleRepository
                .RemoveSubject(subject);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
