using System;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Jogo {

        public int idJogo;

        public Panilha panilha;

        public Conjuntos.CAMPANHA_CAPITULO campanhaCapitulo;

        public int campanhaPagina;

        public DateTime dataCriacao;

        public DateTime dataSalvo;


        public Jogo(int idJogo) {
            this.idJogo = idJogo;
            this.campanhaCapitulo = PaginaUtils.PAGINA_INICIAL().idCapitulo;
            this.campanhaPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
            this.panilha = null;
        }


        public void AjustarSeForNovoJogo() {
            if (dataCriacao == DateTime.MinValue) {
                dataCriacao = DateTime.Now;
                dataSalvo = dataCriacao;
                panilha = null;
            }
            if (campanhaCapitulo == PaginaUtils.PAGINA_ZERADA().idCapitulo) {
                campanhaCapitulo = PaginaUtils.PAGINA_INICIAL().idCapitulo;
                campanhaPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
                panilha = null;
            }
            if ((campanhaCapitulo == PaginaUtils.PAGINA_INICIAL().idCapitulo) && (campanhaPagina < PaginaUtils.PAGINA_INICIAL().idPagina)) {
                campanhaPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
                panilha = null;
            }
        }


        public Jogo Clonar() {
            Jogo _clone = Uteis.Clonar<Jogo>(this);
            if ((_clone.panilha != null) && ((_clone.panilha.habilidadeInicial + _clone.panilha.energiaInicial + _clone.panilha.sorteInicial) == 0))
                _clone.panilha = null;
            return _clone;
        }
    }
}