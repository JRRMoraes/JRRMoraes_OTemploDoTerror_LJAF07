using System;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Jogo {

        public int idJogo;

        public Panilha panilha;

        public int campanhaIdPagina;

        public Conjuntos.CAMPANHA_CAPITULO campanhaIdCapitulo;

        public DateTime dataCriacao;

        public DateTime dataSalvo;


        public Jogo(int idJogo) {
            this.idJogo = idJogo;
            this.campanhaIdCapitulo = PaginaUtils.PAGINA_INICIAL().idCapitulo;
            this.campanhaIdPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
            this.panilha = null;
        }


        public static bool EhValido(Jogo jogo) {
            if (jogo is null)
                return false;
            if (jogo.idJogo == 0)
                return false;
            if (!Panilha.EhValido(jogo.panilha))
                return false;
            if (jogo.campanhaIdCapitulo == PaginaUtils.PAGINA_ZERADA().idCapitulo)
                return false;
            if (jogo.campanhaIdPagina < PaginaUtils.PAGINA_INICIAL().idPagina)
                return false;
            if (jogo.dataCriacao == DateTime.MinValue)
                return false;
            return true;
        }


        public bool AjustarSeForNovoJogo() {
            if (Jogo.EhValido(this))
                return false;
            panilha = null;
            campanhaIdCapitulo = PaginaUtils.PAGINA_INICIAL().idCapitulo;
            campanhaIdPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
            dataCriacao = DateTime.Now;
            dataSalvo = dataCriacao;
            return true;
        }


        public Jogo Clonar() {
            Jogo _clone = Uteis.Clonar<Jogo>(this);
            if (!Panilha.EhValido(_clone.panilha))
                _clone.panilha = null;
            return _clone;
        }
    }
}