#region Documentación
/****************************************************************************************************
* WEBAPI                                                      
****************************************************************************************************
* Unidad        : <.NET/C# para el contexto y la creacion de data inicial en base de datos>                                                                      
* DescripciÓn   : <Logica de negocio para el contexto y la creacion de data inicial en base de datos>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <16-09-2024>                                                                             
***************************************************************************************************/
#endregion Documentación

using Web.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Context
{
    public class WepApiContext : DbContext
    {
        /// <summary>
        /// Propiedad para la obtencion del Dbset de los QuizItem
        /// </summary>
        public DbSet<QuizItemModel> QuizItemModelVirtual { get; set; }

        /// <summary>
        /// Propiedad para la creacion de la base de datos
        /// </summary>
        public WepApiContext(DbContextOptions<WepApiContext> options) : base(options) { }

        /// <summary>
        /// Propiedad para el seteo de data
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<QuizItemModel> quizItemInit = new List<QuizItemModel>();
            quizItemInit.Add(new QuizItemModel() { Id = Guid.Parse("d97c9709-1b2e-4084-aee3-17094f61bf74"), Question = "Which of the following is the name of a Leonardo da Vinci's masterpiece?", AnswerIndex = 2, Score = 3, Theme = "Art", Choices = new List<string> { "Sunflowers", "Mona Lisa", "The Kiss" } });
            quizItemInit.Add(new QuizItemModel() { Id = Guid.Parse("d97c9709-1b2e-4084-aee3-17094f61bf02"), Question = "Which of the following novels was written by Miguel de Cervantes?", AnswerIndex = 3, Score = 3, Theme = "Literature", Choices = new List<string> { "The Ingenious Gentleman Don Quixote of La Mancia", "The Life of Gargantua and of Pantagruel", "One Hundred Years of Solitude" } });

            modelBuilder.Entity<QuizItemModel>(quizItem =>
            {
                quizItem.ToTable("QuizItem");
                quizItem.Property(p => p.Question);
                quizItem.Property(p => p.AnswerIndex);
                quizItem.Property(p => p.Score);
                quizItem.Property(p => p.Theme);
                quizItem.Property(p => p.ChoicesJson)
               .HasColumnName("Choices")
               .HasColumnType("nvarchar(max)");
                quizItem.HasData(quizItemInit);
            }
            );
        }
    }
}
