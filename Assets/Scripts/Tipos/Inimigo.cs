namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Inimigo {

        public string inimigo;

        public int habilidade;

        public int energia;
    }





    [System.Serializable]
    public class InimigoExecucao : Inimigo {
        public int exeIdInimigo;
        public int exeEnergiaAtual;
        public int exeSerieDeAtaqueVencidoConsecutivo;
        public int exeRolagemTotalJogador;
        public int exeRolagemTotalInimigo;
        public int exeRolagemTotalSorte;
        public Conjuntos.RESULTADO_DADOS exeRolagemResultadoAtaque = Conjuntos.RESULTADO_DADOS.NULO;
        public Conjuntos.RESULTADO_DADOS exeRolagemResultadoSorte = Conjuntos.RESULTADO_DADOS.NULO;
    }
}