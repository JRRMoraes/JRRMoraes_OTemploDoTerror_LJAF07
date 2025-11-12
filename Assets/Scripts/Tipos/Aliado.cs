namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Aliado {

        public string aliado;

        public int habilidade;

        public int energia;
    }





    [System.Serializable]
    public class AliadoExecucao : Aliado {

        public int exeEnergiaAtual;

        public bool exeEhAliado;

        public bool exeEstaVivo;
    }
}