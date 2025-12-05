using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.LIB {

    public enum PROCESSO {
        _ZERADO,
        INICIANDO,
        PROCESSANDO,
        CONCLUINDO,
        _FINALIZADO,
        __NULO
    }





    [System.Serializable]
    public class ProcessoMotorIEnumerator {

        public MonoBehaviour monoBehaviour { get; set; }

        public PROCESSO processo { get; private set; } = PROCESSO._ZERADO;

        PROCESSO processoUltimo = PROCESSO.__NULO;

        public bool primeiraVez { get; private set; }

        public Func<bool, bool> rotinaEhValido;

        public List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinasZerado = new List<Func<Action<PROCESSO, bool>, IEnumerator>>();

        public List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinasIniciando = new List<Func<Action<PROCESSO, bool>, IEnumerator>>();

        public List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinasProcessando = new List<Func<Action<PROCESSO, bool>, IEnumerator>>();

        public List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinasConcluindo = new List<Func<Action<PROCESSO, bool>, IEnumerator>>();

        public List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinasFinalizado = new List<Func<Action<PROCESSO, bool>, IEnumerator>>();



        public void ImporProcesso(PROCESSO novoProcesso) {
            processo = novoProcesso;
        }


        public void Processar(PROCESSO novoProcesso) {
            if (monoBehaviour is null)
                return;
            monoBehaviour.StartCoroutine(Interno_Processar(novoProcesso));
        }


        public void Processar() {
            if (monoBehaviour is null)
                return;
            monoBehaviour.StartCoroutine(Interno_Processar(processo));
        }


        public IEnumerator ProcessarIEnumerator(PROCESSO novoProcesso) {
            if (monoBehaviour is null)
                yield break;
            yield return Interno_Processar(novoProcesso);
        }


        public IEnumerator ProcessarIEnumerator() {
            if (monoBehaviour is null)
                yield break;
            yield return Interno_Processar(processo);
        }


        IEnumerator Interno_Processar(PROCESSO novoProcesso) {
            processo = novoProcesso;
            primeiraVez = (processo != processoUltimo);
            processoUltimo = processo;
            if (rotinaEhValido != null) {
                if (!rotinaEhValido.Invoke(primeiraVez))
                    yield break;
            }
            switch (processo) {
                case PROCESSO._ZERADO:
                    yield return Interno_ProcessarRotinas(rotinasZerado);
                    break;
                case PROCESSO.INICIANDO:
                    yield return Interno_ProcessarRotinas(rotinasIniciando);
                    break;
                case PROCESSO.PROCESSANDO:
                    yield return Interno_ProcessarRotinas(rotinasProcessando);
                    break;
                case PROCESSO.CONCLUINDO:
                    yield return Interno_ProcessarRotinas(rotinasConcluindo);
                    break;
                case PROCESSO._FINALIZADO:
                    yield return Interno_ProcessarRotinas(rotinasFinalizado);
                    break;
            }
        }


        IEnumerator Interno_ProcessarRotinas(List<Func<Action<PROCESSO, bool>, IEnumerator>> rotinas) {
            if (rotinas.Count <= 0)
                yield break;
            for (int I = 0; I < rotinas.Count; I++) {
                IEnumerator _rotina = rotinas[I]((_processoAlteradoI, _ehProcessamentoI) => {
                    if (_processoAlteradoI != PROCESSO.__NULO) {
                        if (_ehProcessamentoI)
                            Processar(_processoAlteradoI);
                        else
                            ImporProcesso(_processoAlteradoI);
                    }
                });
                monoBehaviour.StartCoroutine(_rotina);
                yield return null;
            }
        }


        public void IniciarCoroutine(IEnumerator coroutine) {
            if (monoBehaviour is null)
                return;
            monoBehaviour.StartCoroutine(coroutine);
        }
    }
}