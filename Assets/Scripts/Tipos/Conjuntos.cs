namespace Assets.Scripts.Tipos {

    public class Conjuntos {

        public enum CAMPANHA_CAPITULO {
            NULO,
            PAGINAS_INICIAIS,
            PAGINAS_CAMPANHA
        }


        public enum PAGINA_EXECUTOR_ESTADO {
            NULO,
            INICIALIZADO,
            HISTORIAS,
            COMBATE,
            DESTINOS
        }


        public enum JOGO_NIVEL {
            NORMAL,
            FACIL
        }


        public enum PROCESSO {
            ZERO,
            INICIANDO,
            PROCESSANDO,
            CONCLUIDO,
            DESTRUIDO
        }


        public enum ATRIBUTO {
            FUNCAO,
            HABILIDADE,
            ENERGIA,
            SORTE,
            OURO,
            PROVISAO,
            ENCANTOS,
            ITENS
        }


        public enum COMPARACAO {
            POSSUIR,
            NAO_POSSUIR,
            MAIOR_IGUAL,
            MAIOR,
            MENOR_IGUAL,
            MENOR
        }


        public enum POSTURA_INIMIGO {
            AGUARDANDO,
            ATACANTE,
            APOIO,
            MORTO
        }


        public enum RESULTADO_COMBATE {
            NULO,
            COMBATENDO,
            VITORIA,
            DERROTA
        }


        public enum RESULTADO_DADOS {
            NULO,
            VITORIA,
            DERROTA,
            EMPATE
        }


        public enum ATRIBUTO_DESTINO_TESTE {
            NULO,
            HABILIDADE,
            SORTE
        }
    }
}