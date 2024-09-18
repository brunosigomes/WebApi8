using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.Dto.Livro;
using WebApi8.Models;

namespace WebApi8.Services.Livro
{
    public class LivroService : ILivroInterface
    {
        private readonly AppDbContext _context;
        public LivroService(AppDbContext context) { 
            _context = context;
        }

        public async Task<ResponseModel<LivroModel>> BuscarLivroPorId(int idLivro)
        {
            ResponseModel<LivroModel> response = new ResponseModel<LivroModel>();

            try
            {
                var autor = await _context.Livros.FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);

                if (autor == null)
                {
                    response.Mensagem = "Nenhum livro encontrado!";
                }
                else
                {
                    response.Dados = autor;
                    response.Mensagem = "Livro Localizado!";
                }

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<LivroModel>>> BuscarLivroPorIdAutor(int idAutor)
        {
            ResponseModel<List<LivroModel>> response = new ResponseModel<List<LivroModel>>();

            try
            {
                var livros = await _context.Livros
                    .Include(a => a.Autor)
                    .Where(livroBanco => livroBanco.Autor.Id == idAutor)
                    .ToListAsync();

                if (livros == null)
                {
                    response.Mensagem = "Nenhum livro encontrado!";
                }
                else
                {
                    response.Dados = livros;
                    response.Mensagem = "Livros Localizado!";
                }

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<LivroModel>>> CriarLivro(LivroCriacaoDto livroCriacaoDto)
        {
            ResponseModel<List<LivroModel>> response = new ResponseModel<List<LivroModel>>();

            try
            {
                var autor = await _context.Autores
                    .FirstOrDefaultAsync(autorBanco => autorBanco.Id == livroCriacaoDto.Autor.Id);

                if (autor == null)
                {
                    response.Mensagem = "Nenhum registro de autor localizado!";
                } else
                {
                    var livro = new LivroModel()
                    {
                        Titulo = livroCriacaoDto.Titulo,
                        Autor = autor
                    };

                    _context.Add(livro);
                    await _context.SaveChangesAsync();

                    response.Dados = await _context.Livros.Include(a => a.Autor).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<LivroModel>>> EditarLivro(LivroEdicaoDto livroEdicaoDto)
        {
            ResponseModel<List<LivroModel>> response = new ResponseModel<List<LivroModel>>();

            try
            {
                var livro = await _context.Livros
                    .Include(a => a.Autor)
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == livroEdicaoDto.Id);

                var autor = await _context.Autores
                    .FirstOrDefaultAsync(autorBanco => autorBanco.Id == livroEdicaoDto.Autor.Id);

                if (livro == null)
                {
                    response.Mensagem = "Nenhum registro de livro localizado!";
                    return response;
                }

                if (autor == null)
                {
                    response.Mensagem = "Nenhum registro de autor localizado!";
                    return response;
                }
                
                livro.Titulo = livroEdicaoDto.Titulo;
                livro.Autor = autor;

                _context.Update(livro);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Livros.Include(a => a.Autor).ToListAsync();

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<LivroModel>>> ExcluirLivro(int idLivro)
        {
            ResponseModel<List<LivroModel>> response = new ResponseModel<List<LivroModel>>();

            try
            {
                var livro = await _context.Livros.FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);

                if (livro == null)
                {
                    response.Mensagem = "Nenhum livro encontrado!";
                }
                else
                {
                    _context.Remove(livro);
                    await _context.SaveChangesAsync();

                    response.Dados = await _context.Livros.Include(a => a.Autor).ToListAsync();
                    response.Mensagem = "Livro removido com sucesso!";
                }
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<LivroModel>>> ListarLivros()
        {
            ResponseModel<List<LivroModel>> response = new ResponseModel<List<LivroModel>>();

            try
            {
                var livros = await _context.Livros.Include(a => a.Autor).ToListAsync();

                response.Dados = livros;
                response.Mensagem = "Todos os livros foram coletados!";

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }
    }
}
