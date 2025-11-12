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
    }
}