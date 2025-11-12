namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Destino {

        public int idPagina;

        public Conjuntos.CAMPANHA_CAPITULO idCapitulo;

        public string textoDestino;

        public string[] textosDestino;

        public AprovacaoDestino[] aprovacoes;

        public Conjuntos.ATRIBUTO_DESTINO_TESTE testeAtributo;

        public int testeSomarDados;

        public int idPaginaAzar;

        public string imagem;
    }





    [System.Serializable]
    public class DestinoExecucao : Destino {

        public string imagemArquivo;
    }





    [System.Serializable]
    public class AprovacaoDestino {

        public Conjuntos.ATRIBUTO atributoAprovacao;

        public string nomeAprovacao;

        public Conjuntos.COMPARACAO comparacao;

        public int quantidade;
    }
}