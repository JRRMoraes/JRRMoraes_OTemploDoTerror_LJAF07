using Assets.Scripts.Tipos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    public interface IPadraoObservador {

        void AoNotificar(Conjuntos.OBSERVADOR_CONDICAO observadorCondicao);
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


        IEnumerator Interno_Notificar(Conjuntos.OBSERVADOR_CONDICAO observadorCondicao) {
            foreach (var _observador in observadorers)
                _observador.AoNotificar(observadorCondicao);
            yield return new WaitForSeconds(Constantes.TEMPO_OBSERVADOR);
        }


        public void Notificar(Conjuntos.OBSERVADOR_CONDICAO observadorCondicao) {
            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(Interno_Notificar(observadorCondicao));
            else
                Interno_Notificar(observadorCondicao);
        }


        public void Notificar(Conjuntos.OBSERVADOR_CONDICAO[] observadorCondicoes) {
            foreach (var _observadorCondicao in observadorCondicoes)
                Notificar(_observadorCondicao);
        }
    }
}
