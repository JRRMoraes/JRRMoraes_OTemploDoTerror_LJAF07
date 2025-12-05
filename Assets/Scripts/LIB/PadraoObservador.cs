using Assets.Scripts.Tipos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Tipos.Conjuntos;

namespace Assets.Scripts {

    public interface IPadraoObservador {

        void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao);
    }





    public class PadraoObservadorAlvo {

        List<IPadraoObservador> observadorers = new List<IPadraoObservador>();

        public MonoBehaviour monoBehaviour { get; set; }


        public void Inscrever(IPadraoObservador observador) {
            if (!observadorers.Contains(observador))
                observadorers.Add(observador);
        }


        public void Desinscrever(IPadraoObservador observador) {
            if (observadorers.Contains(observador))
                observadorers.Remove(observador);
        }


        IEnumerator Interno_Notificar(OBSERVADOR_CONDICAO observadorCondicao) {
            foreach (var _observador in observadorers) {
                if (_observador != null)
                    _observador.AoNotificar(observadorCondicao);
            }
            yield return new WaitForSeconds(Constantes.TEMPO_OBSERVADOR);
        }


        public void Notificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(Interno_Notificar(observadorCondicao));
            else
                Interno_Notificar(observadorCondicao);
        }


        public void Notificar(OBSERVADOR_CONDICAO[] observadorCondicoes) {
            if ((observadorCondicoes != null) && (observadorCondicoes.Length >= 1)) {
                foreach (var _observadorCondicao in observadorCondicoes)
                    Notificar(_observadorCondicao);
            }
            else {
                foreach (var _observadorCondicao in (OBSERVADOR_CONDICAO[])Enum.GetValues(typeof(OBSERVADOR_CONDICAO)))
                    Notificar(_observadorCondicao);
            }
        }
    }
}
