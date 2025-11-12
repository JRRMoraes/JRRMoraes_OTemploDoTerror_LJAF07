namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Pagina {

        public int idPagina;

        public Conjuntos.CAMPANHA_CAPITULO idCapitulo = Conjuntos.CAMPANHA_CAPITULO.NULO;

        public string titulo;

        public Historia[] historias = new Historia[] { };

        public Combate combate;

        public Destino[] destinos = new Destino[] { };


        public Pagina() { }


        public Pagina(int idPagina, Conjuntos.CAMPANHA_CAPITULO idCapitulo, string titulo) {
            this.idPagina = idPagina;
            this.idCapitulo = idCapitulo;
            this.titulo = titulo;
        }
    }





    public class PaginaUtils {

        public static Pagina PAGINA_ZERADA() {
            return new Pagina(-99999, Conjuntos.CAMPANHA_CAPITULO.NULO, "");
        }


        public static Pagina PAGINA_INICIAL() {
            return new Pagina(1, Conjuntos.CAMPANHA_CAPITULO.PAGINAS_INICIAIS, "No início");
        }


        public static Pagina PAGINA_DETONADO() {
            return new Pagina(99999, Conjuntos.CAMPANHA_CAPITULO.PAGINAS_CAMPANHA, "Parabéns");
        }


        public static Pagina PAGINA_FIM_DE_JOGO() {
            return new Pagina(99444, Conjuntos.CAMPANHA_CAPITULO.PAGINAS_CAMPANHA, "Fim de jogo");
        }
    }
}