namespace Assets.Scripts.Tipos {

    public class Constantes {

        public const float TEMPO_OBSERVADOR = 0.010f;
        public const float TEMPO_ANIMACAO_PEQUENO = 0.200f;
        public const float TEMPO_ANIMACAO_NORMAL = 2.000f;
        public const float TEMPO_ANIMACAO_GRANDE = 4.000f;
        public const int TEMPO_DADOS_ROLANDO_SEGUNDOS = 2;
        public const int TEMPO_DADOS_RESULTADO_MILESIMOS = (TEMPO_DADOS_ROLANDO_SEGUNDOS + 1) * 1000;

        public const int DESENVOLVIMENTO_PADRAO_WIDTH = 1360;
        public const int CELULAR_MIN_WIDTH = 768;
        public const int TABLET_MIN_WIDTH = 1024;
        public const int FLIP_BOOK_ALTURA_MINIMA = 480; /// Considera a página.
        public const int FLIP_BOOK_LARGURA_MINIMA = 360; /// Considera a página.

        public const string COR_HABILIDADE = "#2cb0e4";
        public const string COR_HABILIDADE_DOTS = "#000000";
        public const string COR_ENERGIA = "#d32f2f";
        public const string COR_ENERGIA_DOTS = "#ffffff";
        public const string COR_SORTE = "#800080";
        public const string COR_SORTE_DOTS = "#ffffff";
        public const string COR_OURO = "#ffd700";

        public const int MORTE_DANO_JOGADOR = -999;
        public const int ATAQUE_DANO_JOGADOR = -2;
        public const int ATAQUE_DANO_INIMIGO = -2;
        public const int SORTE_VITORIA_CURA_JOGADOR = 1;
        public const int SORTE_DERROTA_DANO_JOGADOR = -1;
        public const int SORTE_VITORIA_DANO_INIMIGO = -2;
        public const int SORTE_DERROTA_CURA_INIMIGO = 1;
        public const int SORTE_CUSTO_JOGADOR = -1;
        public const int CURAR_CURA_ENERGIA_JOGADOR = 4;
        public const int CURAR_CUSTO_PROVISAO_JOGADOR = -1;

        public const string COMBATE_APROVACAO_DERROTA__SERIE_DE_ATAQUE_EH_MAIOR_OU_IGUAL_A_HABILIDADE = "SerieDeAtaqueEhMaiorOuIgualAHabilidade";
        public const string COMBATE_APROVACAO_DERROTA__INIMIGO_COM_SERIE_DE_ATAQUE_VENCIDO_CONSECUTIVO_2 = "InimigoComSerieDeAtaqueVencidoConsecutivo_2";

        public const float HISTORIA_VELOCIDADE_TEXTO_NORMAL = 0.020f;
        public const float HISTORIA_VELOCIDADE_TEXTO_RAPIDO = 0f;
    }
}