using System;
using System.IO;
using UnityEngine;
using Assets.Scripts.Tipos;


public class ArquivadorDeJogo : MonoBehaviour {

    string NomearArquivo(int pIdJogo) {
        return (Application.persistentDataPath + "/jogoSalvo_" + pIdJogo.ToString() + ".json");
    }


    string NomearArquivo(Jogo pJogoSalvo) {
        return NomearArquivo(pJogoSalvo.idJogo);
    }


    public void SalvarJogo(Jogo pJogoASalvar) {
        string _json = JsonUtility.ToJson(pJogoASalvar);
        File.WriteAllText(NomearArquivo(pJogoASalvar), _json);
    }


    public Jogo CarregarJogo(int pIdJogo) {
        if (File.Exists(NomearArquivo(pIdJogo))) {
            string _json = File.ReadAllText(NomearArquivo(pIdJogo));
            return JsonUtility.FromJson<Jogo>(_json);
        }
        return new Jogo(pIdJogo);
    }
}
