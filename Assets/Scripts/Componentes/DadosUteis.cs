using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    public enum DADO {
        D6_BRANCO,
        D6_PRETO,
        D6_AZUL,
        D6_VERDE,
        D6_VERMELHO,
        D6_DOURADO
    }





    public class DadosUteis {

        public static Sprite[] ObterSpritesDados(DADO dado) {
            switch (dado) {
                case DADO.D6_BRANCO:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Branco_rgbDADADA.png");
                case DADO.D6_PRETO:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Preto_rgb393939.png");
                case DADO.D6_AZUL:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Azul_rgb207FF3.png");
                case DADO.D6_VERDE:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Verde_rgb3CC156.png");
                case DADO.D6_VERMELHO:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Vermelho_rgbFC3637.png");
                case DADO.D6_DOURADO:
                    return Resources.LoadAll<Sprite>("Dados/Dados6_Dourado_rgbFE8E20.png");
            }
            return null;
        }
    }
}