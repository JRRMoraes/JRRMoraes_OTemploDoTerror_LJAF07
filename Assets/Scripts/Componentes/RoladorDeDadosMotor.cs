using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    public class RoladorDeDadosMotor : MonoBehaviour {

        public float intervaloEntreSorteios = 0.2f;

        public int quantidadeDeSorteios = 10;
    }





    [System.Serializable]
    public class RoladorDeDados {

        public RoladorDeDadosMotor roladorDeDadosMotor { get; set; }

        public VisualElement raizVisualElement { get; set; }

        public List<DadoRolado> dadosRolados = new List<DadoRolado>();

        public Action<int[]> OnResultadoFinal;

        int[] resultados;

        int dadosFinalizados;



        bool Interno_Iniciar() {
            resultados = new int[dadosRolados.Count];
            dadosFinalizados = 0;
            if (roladorDeDadosMotor is null)
                return false;
            if (raizVisualElement is null)
                return false;
            if (dadosRolados.Count <= 0)
                return false;
            bool _ok = true;
            int _indiceI = 0;
            foreach (DadoRolado _dadoRoladoI in dadosRolados) {
                _dadoRoladoI.id = _indiceI;
                _dadoRoladoI.spritesDado = DadosUteis.ObterSpritesDados(_dadoRoladoI.dado);
                _dadoRoladoI.elementoImagem = raizVisualElement.Q<VisualElement>(_dadoRoladoI.nomeImagem);
                _dadoRoladoI.OnResultado = RegistrarResultados;
                _dadoRoladoI.ehValido = (_dadoRoladoI.spritesDado != null)
                    && (_dadoRoladoI.spritesDado.Length >= 1)
                    && (_dadoRoladoI.elementoImagem != null);
                _ok &= (_dadoRoladoI.ehValido);
                _indiceI++;
            }
            return _ok;
        }


        void RegistrarResultados(DadoRolado dadoRolado) {
            if (resultados.Length < dadoRolado.id)
                return;
            resultados[dadoRolado.id] = dadoRolado.resultado;
            dadosFinalizados++;
            if (dadosFinalizados == dadosRolados.Count)
                OnResultadoFinal?.Invoke(resultados);
        }


        public void Iniciar() {
            if (!Interno_Iniciar())
                return;
            foreach (DadoRolado _dadoRoladoI in dadosRolados) {
                _dadoRoladoI.elementoImagem.style.backgroundImage = new StyleBackground(_dadoRoladoI.spritesDado[0]);
                //_dadoRoladoI.elementoImagem.style.height = _dadoRoladoI.spritesDado[0].rect.height;
                //_dadoRoladoI.elementoImagem.style.width = (_dadoRoladoI.spritesDado[0].rect.width * 2);
                _dadoRoladoI.elementoImagem.SetEnabled(false);
            }
        }


        public void Rolar() {
            if (!Interno_Iniciar()) {
                OnResultadoFinal?.Invoke(null);
                return;
            }
            foreach (DadoRolado _dadoRoladoI in dadosRolados) {
                _dadoRoladoI.concluido = false;
                _dadoRoladoI.resultado = 0;
                roladorDeDadosMotor.StartCoroutine(RolarDado(_dadoRoladoI));
            }
        }


        IEnumerator RolarDado(DadoRolado dadoRolado) {
            int _resultado = 0;
            int _ultimo = 0;
            dadoRolado.elementoImagem.SetEnabled(true);
            yield return null;
            for (int _sorteioI = 0; _sorteioI < roladorDeDadosMotor.quantidadeDeSorteios; _sorteioI++) {
                int _lado = UnityEngine.Random.Range(1, dadoRolado.ObterLadosDoDado() + 1);
                while (_lado == _ultimo)
                    _lado = UnityEngine.Random.Range(1, dadoRolado.ObterLadosDoDado() + 1);
                dadoRolado.elementoImagem.style.backgroundImage = new StyleBackground(dadoRolado.spritesDado[_lado - 1]);
                _resultado = _lado;
                _ultimo = _lado;
                yield return new WaitForSeconds(roladorDeDadosMotor.intervaloEntreSorteios);
            }
            dadoRolado.concluido = true;
            dadoRolado.resultado = _resultado;
            dadoRolado.OnResultado?.Invoke(dadoRolado);
        }
    }





    [System.Serializable]
    public class DadoRolado {

        public int id { get; set; }

        public DADO dado;

        public Sprite[] spritesDado { get; set; }

        public string nomeImagem;

        public VisualElement elementoImagem { get; set; }

        public Action<DadoRolado> OnResultado { get; set; }

        public bool ehValido { get; set; }

        public bool concluido { get; set; }

        public int resultado { get; set; }



        public DadoRolado(DADO dado, string nomeImagem) {
            this.dado = dado;
            this.nomeImagem = nomeImagem;
        }


        public int ObterLadosDoDado() {
            if (spritesDado is null)
                return 0;
            return spritesDado.Length;
        }
    }
}
