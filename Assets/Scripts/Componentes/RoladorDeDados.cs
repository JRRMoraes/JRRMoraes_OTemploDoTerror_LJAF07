using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    public class RoladorDeDados : MonoBehaviour {

        public UIDocument uiDocument;

        public float intervaloEntreSorteios = 0.2f;

        public int quantidadeDeSorteios = 10;

        public Action<int[]> OnResultadoFinal;

        public List<ElementoDados> elementosDados = new List<ElementoDados>();

        int[] resultados;

        int dadosFinalizados;

        VisualElement raiz;


        void Start() {
            raiz = uiDocument.rootVisualElement;
            if (raiz is null)
                return;
            resultados = new int[elementosDados.Count];
            for (int _indiceI = 0; _indiceI < elementosDados.Count; _indiceI++) {
                elementosDados[_indiceI].roladorDeDados = this;
                elementosDados[_indiceI].imagem = raiz.Q<VisualElement>(elementosDados[_indiceI].nomeImagem);
                elementosDados[_indiceI].OnResultado = (_resultado) => RegistrarResultados(_indiceI, _resultado);
            }
        }


        public void RolarTodos() {
            resultados = new int[elementosDados.Count];
            dadosFinalizados = 0;
            foreach (ElementoDados elementoDadosI in elementosDados)
                elementoDadosI.Rolar();
        }


        void RegistrarResultados(int indice, int resultado) {
            resultados[indice] = resultado;
            dadosFinalizados++;
            if (dadosFinalizados == elementosDados.Count)
                OnResultadoFinal?.Invoke(resultados);
        }





        public class ElementoDados {

            public Sprite[] dado;

            public string nomeImagem;

            public RoladorDeDados roladorDeDados { get; set; }

            public VisualElement imagem { get; set; }

            public Action<int> OnResultado { get; set; }



            public void Rolar() {
                roladorDeDados.StartCoroutine(Rolagem());
            }


            private IEnumerator Rolagem() {
                int _resultado = 0;
                if (dado is null)
                    OnResultado?.Invoke(_resultado);
                for (int i = 0; i < roladorDeDados.quantidadeDeSorteios; i++) {
                    int _lado = UnityEngine.Random.Range(1, dado.Length + 1);
                    imagem.style.backgroundImage = new StyleBackground(dado[_lado - 1]);
                    _resultado = _lado;
                    yield return new WaitForSeconds(roladorDeDados.intervaloEntreSorteios);
                }
                OnResultado?.Invoke(_resultado);
            }
        }
    }
}