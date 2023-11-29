using Data.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _noteService;

        public NotesController(INotesService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notes = await _noteService.GetNotesAsync();
            return Ok(notes);
        }


        /// <summary>
        /// Get note by id
        /// </summary>
        /// <param name="id" example="1">note id</param>
        [HttpGet("{id}", Name = "GetNote")]
        public async Task<IActionResult> Get(int id)
        {
            var note = await _noteService.GetNoteAsync(id);
            if (!note.IsSuccess)
                return NotFound(note.Errors);

            return Ok(note);
        }


        /// <summary>
        /// add new note
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(NoteDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _noteService.AddNote(model);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            var note = result.Data as NoteDTO;


            return CreatedAtRoute("GetNote", new { id = note.Id }, result);
        }


        /// <summary>
        /// update note
        /// </summary>
        /// <param name="id">note id</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, NoteDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            model.Id = id;

            var result = await _noteService.UpdateNoteAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok();
        }


        /// <summary>
        /// delete note
        /// </summary>
        /// <param name="id" example="1">note id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.DeleteNoteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok();
        }


    }
}
