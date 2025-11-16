using System;
using System.Linq;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Panilha {

        public string nome;

        public Conjuntos.JOGO_NIVEL nivel;

        public int habilidade;

        public int habilidadeInicial;

        public int energia;

        public int energiaInicial;

        public int sorte;

        public int sorteInicial;

        public int ouro;

        public int provisao;

        public Item[] itens = new Item[] { };

        public Encanto[] encantos = new Encanto[] { };



        public static bool EhValido(Panilha panilha) {
            if (panilha is null)
                return false;
            if (panilha.habilidadeInicial <= 0)
                return false;
            if (panilha.energiaInicial <= 0)
                return false;
            if (panilha.sorteInicial <= 0)
                return false;
            return true;
        }


        public static Panilha CriarPanilhaViaRolagens(DadosRoladosTotaisParaPanilhaNova totaisRolados, string nome, Conjuntos.JOGO_NIVEL nivel) {
            Panilha _panilha = new Panilha();
            _panilha.nome = nome;
            _panilha.nivel = nivel;
            _panilha.habilidade = totaisRolados.habilidade;
            _panilha.habilidadeInicial = totaisRolados.habilidade;
            _panilha.energia = totaisRolados.energia;
            _panilha.energiaInicial = totaisRolados.energia;
            _panilha.sorte = totaisRolados.sorte;
            _panilha.sorteInicial = totaisRolados.sorte;
            _panilha.ouro = 0;
            _panilha.provisao = 0;
            _panilha.itens = new Item[] { };
            _panilha.encantos = new Encanto[] { };
            return _panilha;
        }


        public void RetornarPanilhaItensAtualizados(Efeito efeitoProcessado) {
            if (string.IsNullOrWhiteSpace(efeitoProcessado.nomeEfeito))
                return;
            Item _item = itens.FirstOrDefault((itemI) => Uteis.TextosIguais(itemI.idItem, efeitoProcessado.nomeEfeito));
            if (_item != null) {
                _item.quantidade = Math.Max((_item.quantidade + efeitoProcessado.quantidade), 0);
            }
            else if (efeitoProcessado.quantidade > 0) {
                _item = new Item(efeitoProcessado.nomeEfeito, efeitoProcessado.quantidade);
                itens = Uteis.AdicionarNoArray(itens, _item);
            }
            itens = itens.Where((itemI) => itemI.quantidade > 0).ToArray();
        }


        public void RetornarPanilhaEncantosAtualizados(Efeito efeitoProcessado) {
            if (string.IsNullOrWhiteSpace(efeitoProcessado.nomeEfeito))
                return;
            Encanto _encanto = encantos.FirstOrDefault((encantoI) => Uteis.TextosIguais(encantoI.idEncanto, efeitoProcessado.nomeEfeito));
            if (_encanto != null) {
                _encanto.idEncanto = efeitoProcessado.quantidade >= 1 ? efeitoProcessado.nomeEfeito : "";
            }
            else if (efeitoProcessado.quantidade > 0) {
                _encanto = new Encanto(efeitoProcessado.nomeEfeito);
                encantos = Uteis.AdicionarNoArray(encantos, _encanto);
            }
            encantos = encantos.Where((encantoI) => !string.IsNullOrWhiteSpace(encantoI.idEncanto)).ToArray();
        }
    }
}