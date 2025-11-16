using System;
using static Assets.Scripts.Tipos.Conjuntos;

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


        public static InimigoExecucao CriarCom(Inimigo inimigo, int indice) {
            InimigoExecucao _inimigoExecucao = new InimigoExecucao();
            _inimigoExecucao.inimigo = inimigo.inimigo;
            _inimigoExecucao.habilidade = inimigo.habilidade;
            _inimigoExecucao.energia = inimigo.energia;
            _inimigoExecucao.exeEnergiaAtual = _inimigoExecucao.energia;
            _inimigoExecucao.exeIdInimigo = indice;
            return _inimigoExecucao;
        }
    }
}