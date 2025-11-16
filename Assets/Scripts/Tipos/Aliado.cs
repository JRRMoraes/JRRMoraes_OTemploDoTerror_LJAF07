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


        public static AliadoExecucao CriarCom(Aliado aliado) {
            if (aliado is null)
                return null;
            AliadoExecucao _aliadoExecucao = new AliadoExecucao();
            _aliadoExecucao.aliado = aliado.aliado;
            _aliadoExecucao.habilidade = aliado.habilidade;
            _aliadoExecucao.energia = aliado.energia;
            _aliadoExecucao.exeEnergiaAtual = _aliadoExecucao.energia;
            _aliadoExecucao.exeEhAliado = true;
            _aliadoExecucao.exeEstaVivo = true;
            return _aliadoExecucao;
        }
    }
}