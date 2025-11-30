using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Destino {

        public int idPagina;

        public Conjuntos.CAMPANHA_CAPITULO idCapitulo;

        public string textoDestino;

        public string[] textosDestino;

        public AprovacaoDestino[] aprovacoes;

        public Conjuntos.ATRIBUTO_DESTINO_TESTE testeAtributo;

        public int testeSomarDados;

        public int idPaginaAzar;

        public string imagem;
    }





    [System.Serializable]
    public class DestinoExecucao : Destino {

        public string imagemArquivo;

        public Button selecaoButton;


        public static DestinoExecucao CriarCom(Destino destino) {
            DestinoExecucao _destinoExecucao = new DestinoExecucao();
            _destinoExecucao.idPagina = destino.idPagina;
            _destinoExecucao.idCapitulo = destino.idCapitulo;
            if (_destinoExecucao.idCapitulo == Conjuntos.CAMPANHA_CAPITULO.NULO)
                _destinoExecucao.idCapitulo = LivroJogo.INSTANCIA.jogoAtual.campanhaIdCapitulo;
            _destinoExecucao.textoDestino = destino.textoDestino;
            _destinoExecucao.textosDestino = destino.textosDestino;
            _destinoExecucao.aprovacoes = destino.aprovacoes;
            _destinoExecucao.testeAtributo = destino.testeAtributo;
            _destinoExecucao.testeSomarDados = destino.testeSomarDados;
            _destinoExecucao.idPaginaAzar = destino.idPaginaAzar;
            _destinoExecucao.imagem = destino.imagem;
            if (!string.IsNullOrWhiteSpace(_destinoExecucao.imagem)) {
                _destinoExecucao.imagemArquivo = LivroJogo.MontarArquivoECaminho(LivroJogo.IMAGEM_CAMINHO_LIVRO_JOGO, _destinoExecucao.imagem + LivroJogo.IMAGEM_EXTENSAO);
                if (!File.Exists(_destinoExecucao.imagemArquivo)) {
                    Debug.LogError($"Arquivo Imagem Destino não encontrado: {_destinoExecucao.imagemArquivo}");
                    _destinoExecucao.imagemArquivo = "";
                }
            }
            return _destinoExecucao;
        }
    }





    [System.Serializable]
    public class AprovacaoDestino {

        public Conjuntos.ATRIBUTO atributoAprovacao;

        public string nomeAprovacao;

        public Conjuntos.COMPARACAO comparacao;

        public int quantidade;
    }
}