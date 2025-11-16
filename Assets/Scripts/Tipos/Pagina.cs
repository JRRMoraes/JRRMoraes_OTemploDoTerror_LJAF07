using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Pagina {

        public CAMPANHA_CAPITULO idCapitulo = CAMPANHA_CAPITULO.NULO;

        public int idPagina;

        public string titulo;

        public Historia[] historias;

        public Combate combate;

        public Destino[] destinos;


        public Pagina() { }


        public Pagina(int idPagina, CAMPANHA_CAPITULO idCapitulo, string titulo) {
            this.idPagina = idPagina;
            this.idCapitulo = idCapitulo;
            this.titulo = titulo;
        }


        public static bool EhValido(Pagina pagina) {
            if (pagina is null)
                return false;
            if (pagina.idCapitulo == CAMPANHA_CAPITULO.NULO)
                return false;
            if (pagina.idPagina == 0)
                return false;
            if (pagina.idPagina == PaginaUtils.PAGINA_ZERADA().idPagina)
                return false;
            if ((pagina.historias is null) || (pagina.historias.Length <= 0))
                return false;
            return true;
        }
    }





    public class PaginaUtils {

        public static Pagina PAGINA_ZERADA() {
            return new Pagina(-99999, CAMPANHA_CAPITULO.NULO, "");
        }


        public static Pagina PAGINA_INICIAL() {
            return new Pagina(1, CAMPANHA_CAPITULO.PAGINAS_INICIAIS, "No início");
        }


        public static Pagina PAGINA_DETONADO() {
            return new Pagina(99999, CAMPANHA_CAPITULO.PAGINAS_CAMPANHA, "Parabéns");
        }


        public static Pagina PAGINA_FIM_DE_JOGO() {
            return new Pagina(99444, CAMPANHA_CAPITULO.PAGINAS_CAMPANHA, "Fim de jogo");
        }
    }
}