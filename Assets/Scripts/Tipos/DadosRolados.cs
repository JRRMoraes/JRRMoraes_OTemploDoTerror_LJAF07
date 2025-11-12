namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class DadosRoladosParaPanilhaNova {

        public int habilidade1;

        public int energia1;

        public int energia2;

        public int sorte1;
    }


    [System.Serializable]
    public class DadosRoladosTotaisParaPanilhaNova {

        public int habilidade;

        public int energia;

        public int sorte;
    }


    [System.Serializable]
    public class RolagemDeDadosParaPanilhaNova {

        public Conjuntos.PROCESSO processoRolagem = Conjuntos.PROCESSO.ZERO;

        public DadosRoladosParaPanilhaNova rolagens;

        public DadosRoladosTotaisParaPanilhaNova totais;
    }
}