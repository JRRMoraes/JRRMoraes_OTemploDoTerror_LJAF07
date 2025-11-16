namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Livro {

        public string idLivroJogo;

        public string titulo;

        public string autor;

        public string capa;

        public string[] copyright;

        public Apresentacao[] apresentacoes;

        public Pagina[] paginasIniciais;

        public Pagina[] paginasCampanha;


        public static bool EhValido(Livro livro) {
            if (livro is null)
                return false;
            if (string.IsNullOrWhiteSpace(livro.idLivroJogo))
                return false;
            if (string.IsNullOrWhiteSpace(livro.titulo))
                return false;
            if ((livro.paginasIniciais is null) || (livro.paginasIniciais.Length <= 0))
                return false;
            if ((livro.paginasCampanha is null) || (livro.paginasCampanha.Length <= 0))
                return false;
            return true;
        }
    }
}