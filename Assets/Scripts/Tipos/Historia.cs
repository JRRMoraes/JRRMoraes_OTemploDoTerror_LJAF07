namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Historia {

        public string[] textosHistoria;

        public Efeito[] efeitos;

        public string imagem;


        public Historia() {
            textosHistoria = new string[0];
            efeitos = new Efeito[0];
            imagem = "";
        }
    }





    [System.Serializable]
    public class HistoriaTextoExecucao {

        public string[] textosHistoria;

        public Conjuntos.PROCESSO exeProcessoTexto;


        public HistoriaTextoExecucao() {
            textosHistoria = new string[0];
            exeProcessoTexto = Conjuntos.PROCESSO.ZERO;
        }
    }





    [System.Serializable]
    public class HistoriaEfeitoExecucao {

        public Efeito[] efeitos;

        public Conjuntos.PROCESSO exeProcessoEfeito;


        public HistoriaEfeitoExecucao() {
            efeitos = new Efeito[0];
            exeProcessoEfeito = Conjuntos.PROCESSO.ZERO;
        }
    }





    [System.Serializable]
    public class HistoriaImagemExecucao {

        public string imagem;

        public string arquivo;

        public Conjuntos.PROCESSO exeProcessoImagem;


        public HistoriaImagemExecucao() {
            imagem = "";
            arquivo = "";
            exeProcessoImagem = Conjuntos.PROCESSO.ZERO;
        }
    }
}