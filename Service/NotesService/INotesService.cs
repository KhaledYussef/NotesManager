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
        Task<ResponseResult> GetSharedNoteAsync(string shareLink);
        Task<bool> IsNoteBelongsToUserAsync(int noteId);
        Task<bool> IsNoteSharedAsync(string shareLink);
        Task<ResponseResult> ShareNoteAsync(int noteId, string shareLink);
        Task<ResponseResult> UpdateNoteAsync(NoteDTO model);
    }
}