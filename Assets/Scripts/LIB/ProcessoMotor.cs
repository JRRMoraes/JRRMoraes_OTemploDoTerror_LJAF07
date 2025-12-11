using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.LIB {

    public enum PROCESSO {
        _ZERADO,
        _ZERADO_EXEC,
        INICIANDO,
        INICIANDO_EXEC,
        PROCESSANDO,
        PROCESSANDO_EXEC,
        CONCLUINDO,
        CONCLUINDO_EXEC,
        _FINALIZADO,
        _FINALIZADO_EXEC,
        __NULO
    }





    [System.Serializable]
    public class ProcessoMotor {

        public MonoBehaviour monoBehaviour { get; set; }

        public PROCESSO processo { get { return _processo; } }
        [SerializeField]
        PROCESSO _processo = PROCESSO._ZERADO;

        PROCESSO processoUltimo = PROCESSO.__NULO;

        public bool primeiraVez { get; private set; }

        public Func<bool, bool> rotinaEhValido;

        public List<Func<Action<PROCESSO>, IEnumerator>> rotinasZerado = new List<Func<Action<PROCESSO>, IEnumerator>>();

        public List<Func<Action<PROCESSO>, IEnumerator>> rotinasIniciando = new List<Func<Action<PROCESSO>, IEnumerator>>();

        public List<Func<Action<PROCESSO>, IEnumerator>> rotinasProcessando = new List<Func<Action<PROCESSO>, IEnumerator>>();

        public List<Func<Action<PROCESSO>, IEnumerator>> rotinasConcluindo = new List<Func<Action<PROCESSO>, IEnumerator>>();

        public List<Func<Action<PROCESSO>, IEnumerator>> rotinasFinalizado = new List<Func<Action<PROCESSO>, IEnumerator>>();

        public List<Task<PROCESSO>> tarefasZerado = new List<Task<PROCESSO>>();

        public List<Task<PROCESSO>> tarefasIniciando = new List<Task<PROCESSO>>();

        public List<Task<PROCESSO>> tarefasProcessando = new List<Task<PROCESSO>>();

        public List<Task<PROCESSO>> tarefasConcluindo = new List<Task<PROCESSO>>();

        public List<Task<PROCESSO>> tarefasFinalizado = new List<Task<PROCESSO>>();


        public void ImporProcesso(PROCESSO novoProcesso) {
            _processo = novoProcesso;
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
            _processo = novoProcesso;
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


        IEnumerator Interno_ProcessarRotinas(List<Func<Action<PROCESSO>, IEnumerator>> rotinas) {
            for (int I = 0; I < rotinas.Count; I++) {
                IEnumerator _rotina = rotinas[I](_processoRetornadoI => {
                    if (_processoRetornadoI != PROCESSO.__NULO)
                        Processar(_processoRetornadoI);
                });
                monoBehaviour.StartCoroutine(_rotina);
                yield return null;
            }
        }


        IEnumerator Interno_ProcessarTarefas(List<Task<PROCESSO>> tarefas) {
            for (int I = 0; I < tarefas.Count; I++) {
                Task<PROCESSO> _tarefa = tarefas[I];
                while (!_tarefa.IsCompleted)
                    yield return null;
                if (_tarefa.Result != PROCESSO.__NULO)
                    Processar(_tarefa.Result);
            }
        }


        public void IniciarCoroutine(IEnumerator coroutine) {
            if (monoBehaviour is null)
                return;
            monoBehaviour.StartCoroutine(coroutine);
        }
    }
}