using System;
using System.IO;
using UnityEngine;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Historia {

        public string[] textosHistoria;

        public Efeito[] efeitos;

        public string imagem;
    }





    [System.Serializable]
    public class HistoriaTextoExecucao {

        public string[] textosHistoria;

        public Conjuntos.PROCESSO exeProcessoTexto = Conjuntos.PROCESSO.ZERO;


        public static HistoriaTextoExecucao CriarCom(string[] textosHistoria) {
            HistoriaTextoExecucao _historiaTextoExecucao = new HistoriaTextoExecucao();
            _historiaTextoExecucao.textosHistoria = textosHistoria;
            if (_historiaTextoExecucao.textosHistoria is null)
                _historiaTextoExecucao.textosHistoria = new string[] { };
            return _historiaTextoExecucao;
        }
    }





    [System.Serializable]
    public class HistoriaEfeitoExecucao {

        public EfeitoExecucao[] efeitos;

        public Conjuntos.PROCESSO exeProcessoEfeito = Conjuntos.PROCESSO.ZERO;


        public static HistoriaEfeitoExecucao CriarCom(Efeito[] efeitos) {
            HistoriaEfeitoExecucao _historiaEfeitoExecucao = new HistoriaEfeitoExecucao();
            if (efeitos != null) {
                foreach (Efeito _efeitoI in efeitos) {
                    _historiaEfeitoExecucao.efeitos = Uteis.AdicionarNoArray<EfeitoExecucao>(_historiaEfeitoExecucao.efeitos,
                        EfeitoExecucao.CriarCom(_efeitoI));
                }
            }
            else {
                _historiaEfeitoExecucao.efeitos = new EfeitoExecucao[] { };
            }
            return _historiaEfeitoExecucao;
        }
    }





    [System.Serializable]
    public class HistoriaImagemExecucao {

        public string imagem;

        public string arquivo;

        public Conjuntos.PROCESSO exeProcessoImagem = Conjuntos.PROCESSO.ZERO;


        public static HistoriaImagemExecucao CriarCom(string imagem) {
            HistoriaImagemExecucao _historiaImagemExecucao = new HistoriaImagemExecucao();
            _historiaImagemExecucao.imagem = imagem;
            if (!string.IsNullOrWhiteSpace(_historiaImagemExecucao.imagem)) {
                _historiaImagemExecucao.arquivo = LivroJogo.MontarArquivoECaminho(LivroJogo.IMAGEM_CAMINHO_LIVRO_JOGO, _historiaImagemExecucao.imagem + LivroJogo.IMAGEM_EXTENSAO);
                if (!File.Exists(_historiaImagemExecucao.arquivo)) {
                    Debug.LogError($"Arquivo Imagem não encontrado: {_historiaImagemExecucao.arquivo}");
                    _historiaImagemExecucao.arquivo = null;
                }
            }
            return _historiaImagemExecucao;
        }
    }
}