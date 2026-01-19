namespace ElMosa3ed.Api.Services;
public interface IAiService { Task<string> Ask(string prompt); }