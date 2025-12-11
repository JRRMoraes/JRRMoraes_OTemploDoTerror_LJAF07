using Assets.Scripts.Tipos;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;

namespace Assets.Scripts.Componentes {
    public class TelaCombate : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement combateGroupBox;


        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public PaginaExecutora PaginaExecutoraAtual() {
            return LivroJogo.INSTANCIA.paginaExecutora;
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (combateGroupBox is null)
                combateGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("CombateGroupBox");

            AoNotificar_ProcessarTelaCombate(observadorCondicao);
        }


        bool AoNotificar_ProcessarTelaCombate(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            if (combateGroupBox is null)
                return false;
            if (!PaginaExecutora.EhValido(PaginaExecutoraAtual())) {
                while (combateGroupBox.childCount >= 1)
                    combateGroupBox.Remove(combateGroupBox.Children().First());
                return true;
            }
            if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.COMBATE)
                return false;
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false))
                return false;

            switch (PaginaExecutoraAtual().combateProcesso) {
                case PROCESSO2.ZERO:
                    //if (PaginaExecutoraAtual().combate != null)
                    //    PaginaExecutoraAtual().combateProcesso = PROCESSO.INICIANDO;
                    //else
                    PaginaExecutoraAtual().combateProcesso = PROCESSO2.CONCLUIDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO2.CONCLUIDO:
                    PaginaExecutoraAtual().combateProcesso = PROCESSO2.DESTRUIDO;
                    PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.DESTINOS;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
            }
            return false;
        }
    }
}