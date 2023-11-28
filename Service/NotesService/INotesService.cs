using Core.Domains;

using Data.DTOs;

namespace Service
{
    public interface INotesService
    {
        Task<ResponseResult> AddNote(NoteDTO model);
        Task<ResponseResult> DeleteNoteAsync(int id);
        Task<ResponseResult> GetNoteAsync(int id);
        Task<ResponseResult> GetNotesAsync();
        Task<bool> IsNoteBelongsToUserAsync(int noteId);
        Task<ResponseResult> UpdateNoteAsync(NoteDTO model);
    }
}