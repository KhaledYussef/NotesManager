using Core.Domains;

using Data.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pal.Web.Extensions;

using Service;

namespace Website.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }


        public async Task<IActionResult> Index()
        {
            var notesResult = await _notesService.GetNotesAsync();
            if (notesResult.IsSuccess)
            {
                return View(notesResult.Data);
            }

            return View();
        }

        [ServiceFilter(typeof(CheckUserAttribute))]
        public async Task<IActionResult> Manage(int? id)
        {
            if (id.HasValue)
            {
                var noteResult = await _notesService.GetNoteAsync(id.Value);
                if (noteResult.IsSuccess)
                {
                    return View(noteResult.Data);
                }
                else
                {
                    return RedirectToAction("Index"); //TODO: Show Error
                }

            }

            return View(new NoteDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(NoteDTO model)
        {

            if (ModelState.IsValid)
            {
                ResponseResult result;
                if (model.Id == 0) //Add
                {
                    result = await _notesService.AddNote(model);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else //Update
                {
                    result = await _notesService.UpdateNoteAsync(model);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError("", result.Errors.FirstOrDefault());
            }

            return View("Add", model);

        }


        [HttpPost]
        [ServiceFilter(typeof(CheckUserAttribute))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _notesService.DeleteNoteAsync(id);
            return Json(result);
        }


    }
}
