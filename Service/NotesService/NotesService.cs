using Core.Domains;
using Core.Domains.Notes;
using Core.Enums;

using Data.DbContext;
using Data.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Pal.Services.FileManager;

using Services;
using Services.LoggerService;

namespace Service
{
    public class NotesService : BaseService<NotesService>, INotesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileManagerService _fileManagerService;
        public NotesService(AppDbContext dbContext,
            ILoggerService<NotesService> logger,
            IHttpContextAccessor httpAccessor,
            IFileManagerService fileManagerService) : base(dbContext, logger, httpAccessor)
        {
            _dbContext = dbContext;
            _fileManagerService = fileManagerService;
        }





        /// <summary>
        /// Add Note
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AddNote(NoteDTO model)
        {
            try
            {
                var note = new Note
                {
                    Title = model.Title,
                    Content = model.Content,
                    Color = model.Color,
                    UserId = UserId,
                };

                if (model.ImageFile != null)
                {
                    var imageUrl = await _fileManagerService.UploadFileAsync(model.ImageFile, FileReferenceType.NoteImage, true);
                    note.ImageUrl = imageUrl;
                }

                await _dbContext.Notes.AddAsync(note);
                await _dbContext.SaveChangesAsync();
                model.Id = note.Id;
                return Success(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        /// <summary>
        /// Update Note
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> UpdateNoteAsync(NoteDTO model)
        {
            try
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == model.Id && x.UserId == UserId);
                if (note == null)
                    return Error("Note not found");

                note.Title = model.Title;
                note.Content = model.Content;
                note.Color = model.Color;

                if (model.ImageFile != null)
                {
                    var imageUrl = await _fileManagerService.UploadFileAsync(model.ImageFile, FileReferenceType.NoteImage, true);
                    note.ImageUrl = imageUrl;
                }

                await _dbContext.SaveChangesAsync();
                return Success(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Delete Note
        /// </summary>
        /// <param name="id">Note Id</param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteNoteAsync(int id)
        {
            try
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
                if (note == null)
                    return Error("Note not found");

                note.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Get Note By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetNoteAsync(int id)
        {
            try
            {
                var note = await _dbContext.Notes
                    .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

                if (note == null)
                    return Error("Note not found");

                var model = new NoteDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    Color = note.Color,
                    ImageUrl = note.ImageUrl,
                };

                return Success(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        /// <summary>
        /// Get All Notes
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetNotesAsync()
        {
            try
            {
                var notes = await _dbContext.Notes.AsNoTracking()
                    .Where(x => x.UserId == UserId)
                    .ToListAsync();

                var model = new List<NoteDTO>();
                foreach (var note in notes)
                {
                    model.Add(new NoteDTO
                    {
                        Id = note.Id,
                        Title = note.Title,
                        Content = note.Content,
                        Color = note.Color,
                        ImageUrl = note.ImageUrl,
                    });
                }

                return Success(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        // check if belongs to userid parameter
        public async Task<bool> IsNoteBelongsToUserAsync(int noteId)
        {
            var exist = await _dbContext.Notes.AnyAsync(x => x.Id == noteId && x.UserId == UserId);
            return exist;
        }


    }
}
