using UnityEngine;


namespace Assets.Scripts {
    public class AudioExecutor : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }



        /*
         * 
         export interface IAudioExecutor {
            mudo: boolean;
            audioMusicaRef: RefObject<HTMLAudioElement>;
            volumeMusica: number;
            audioEfeitoRef: RefObject<HTMLAudioElement>;
            volumeEfeito: number;
        }

        export interface IAudioMusica {
            momento: EAudioMomentoMusica;
            atual: string;
        }

        export interface IAudioEfeito {
            momento: EAudioMomentoEfeitoSonoro;
            atual: string;
            tocando: boolean;
        }

        export enum EAudioMomentoMusica {
            _NULO,
            ABERTURA,
            CAMPANHA,
            COMBATE,
            VITORIA_COMBATE,
            VITORIA_JOGO,
            DERROTA_COMBATE,
            DERROTA_JOGO,
        }

        export enum EAudioMomentoEfeitoSonoro {
            ROLANDO_DADOS,
            MUDANDO_PAGINA,
            VITORIA_SOBRE_INIMIGO,
            VITORIA_SOBRE_SERIE_ATAQUE,
            DERROTA_SOBRE_SERIE_ATAQUE,
        }
         * 
         * */
    }
}