using System;
using static Assets.Scripts.Tipos.Conjuntos;

namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Efeito {

        public string textoEfeito;

        public Conjuntos.ATRIBUTO atributoEfeito;

        public string nomeEfeito;

        public int quantidade;
    }





    [System.Serializable]
    public class EfeitoExecucao : Efeito {

        public Conjuntos.PROCESSO2 exeProcessoEfeito;

        public int exeIdEfeito;


        public static EfeitoExecucao CriarCom(Efeito efeito) {
            EfeitoExecucao _efeitoExecucao = new EfeitoExecucao();
            _efeitoExecucao.textoEfeito = efeito.textoEfeito;
            _efeitoExecucao.atributoEfeito = efeito.atributoEfeito;
            _efeitoExecucao.nomeEfeito = efeito.nomeEfeito;
            _efeitoExecucao.quantidade = efeito.quantidade;
            _efeitoExecucao.exeProcessoEfeito = PROCESSO2.ZERO;
            _efeitoExecucao.exeIdEfeito = UnityEngine.Random.Range(1, 1000);
            return _efeitoExecucao;
        }
    }





    [System.Serializable]
    public class EfeitoInimigoExecucao : EfeitoExecucao {

        public int exeIdInimigo;


        public static EfeitoInimigoExecucao CriarCom(Inimigo inimigoI, int indiceInimigo) {
            EfeitoInimigoExecucao _efeitoInimigoExecucao = new EfeitoInimigoExecucao();
            _efeitoInimigoExecucao.exeIdInimigo = indiceInimigo;
            return _efeitoInimigoExecucao;
        }
    }





    public class EfeitoUtils {

        public EfeitoExecucao[] ObterEfeitosExecucaoDeEnergia(int idEfeito, string texto, int quantidade) {
            EfeitoExecucao _efeito = new EfeitoExecucao();
            _efeito.atributoEfeito = Conjuntos.ATRIBUTO.ENERGIA;
            _efeito.textoEfeito = texto;
            _efeito.nomeEfeito = "";
            _efeito.quantidade = quantidade;
            _efeito.exeProcessoEfeito = Conjuntos.PROCESSO2.ZERO;
            _efeito.exeIdEfeito = idEfeito;
            return new EfeitoExecucao[] { _efeito };
        }


        public EfeitoExecucao[] EFEITO_MORTE_NO_JOGADOR() {
            return ObterEfeitosExecucaoDeEnergia(99000, "MORTE!!!", Constantes.MORTE_DANO_JOGADOR);
        }


        public EfeitoExecucao[] EFEITO_ATAQUE_NO_JOGADOR() {
            return ObterEfeitosExecucaoDeEnergia(99001, "ATAQUE!!!", Constantes.ATAQUE_DANO_JOGADOR);
        }


        public EfeitoExecucao[] EFEITO_SORTE_VITORIA_EM_DEFESA_DO_JOGADOR() {
            return ObterEfeitosExecucaoDeEnergia(99002, "SORTE!!!", Constantes.SORTE_VITORIA_CURA_JOGADOR);
        }


        public EfeitoExecucao[] EFEITO_SORTE_DERROTA_EM_DEFESA_DO_JOGADOR() {
            return ObterEfeitosExecucaoDeEnergia(99003, "AZAR!!!", Constantes.SORTE_DERROTA_DANO_JOGADOR);
        }


        public EfeitoExecucao[] EFEITO_SORTE_CUSTO_NO_JOGADOR() {
            EfeitoExecucao _efeito = new EfeitoExecucao();
            _efeito.atributoEfeito = Conjuntos.ATRIBUTO.SORTE;
            _efeito.textoEfeito = "CUSTO DA SORTE";
            _efeito.nomeEfeito = "";
            _efeito.quantidade = Constantes.SORTE_CUSTO_JOGADOR;
            _efeito.exeProcessoEfeito = Conjuntos.PROCESSO2.ZERO;
            _efeito.exeIdEfeito = 99004;
            return new EfeitoExecucao[] { _efeito };
        }


        public EfeitoExecucao[] EFEITOS_CURA_VIA_PROVISAO_NO_JOGADOR(int provisao) {
            if (provisao <= 0)
                return new EfeitoExecucao[] { };
            EfeitoExecucao _efeito1 = new EfeitoExecucao();
            _efeito1.atributoEfeito = Conjuntos.ATRIBUTO.ENERGIA;
            _efeito1.textoEfeito = "CURANDO";
            _efeito1.nomeEfeito = "";
            _efeito1.quantidade = Constantes.CURAR_CURA_ENERGIA_JOGADOR;
            _efeito1.exeProcessoEfeito = Conjuntos.PROCESSO2.ZERO;
            _efeito1.exeIdEfeito = 99100 + provisao;
            EfeitoExecucao _efeito2 = new EfeitoExecucao();
            _efeito2.atributoEfeito = Conjuntos.ATRIBUTO.PROVISAO;
            _efeito2.textoEfeito = "CUSTO DA CURA";
            _efeito2.nomeEfeito = "";
            _efeito2.quantidade = Constantes.CURAR_CUSTO_PROVISAO_JOGADOR;
            _efeito2.exeProcessoEfeito = Conjuntos.PROCESSO2.ZERO;
            _efeito2.exeIdEfeito = 99150 + provisao;
            return new EfeitoExecucao[] { _efeito1, _efeito2 };
        }


        public EfeitoInimigoExecucao[] ObterEfeitosInimigoExecucaoDeEnergia(int idEfeito, int idInimigo, string texto, int quantidade) {
            EfeitoInimigoExecucao _efeito = new EfeitoInimigoExecucao();
            _efeito.atributoEfeito = Conjuntos.ATRIBUTO.ENERGIA;
            _efeito.textoEfeito = texto;
            _efeito.nomeEfeito = "";
            _efeito.quantidade = quantidade;
            _efeito.exeProcessoEfeito = Conjuntos.PROCESSO2.ZERO;
            _efeito.exeIdEfeito = idEfeito;
            _efeito.exeIdInimigo = idInimigo;
            return new EfeitoInimigoExecucao[] { _efeito };
        }


        public EfeitoInimigoExecucao[] EFEITO_ATAQUE_NO_INIMIGO(int idInimigo) {
            return ObterEfeitosInimigoExecucaoDeEnergia(99201, idInimigo, "ATAQUE!!!", Constantes.ATAQUE_DANO_INIMIGO);
        }


        public EfeitoInimigoExecucao[] EFEITO_SORTE_VITORIA_EM_ATAQUE_NO_INIMIGO(int idInimigo) {
            return ObterEfeitosInimigoExecucaoDeEnergia(99202, idInimigo, "SORTE!!!", Constantes.SORTE_VITORIA_DANO_INIMIGO);
        }


        public EfeitoInimigoExecucao[] EFEITO_SORTE_DERROTA_EM_ATAQUE_NO_INIMIGO(int idInimigo) {
            return ObterEfeitosInimigoExecucaoDeEnergia(99203, idInimigo, "AZAR!!!", Constantes.SORTE_DERROTA_CURA_INIMIGO);
        }
    }
}
