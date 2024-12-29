<template>
    <div class="container-md">
        <h1 class="page-title">
            Instrukcja instalacji aplikacji na urządzeniu mobilnym
        </h1>
        <p>
            Aby korzystać z aplikacji FungEye na urządzeniu mobilnym, zainstaluj ją na swoim telefonie. Poniżej
            znajdziesz instrukcje instalacji na systemach Android i iOS.
        </p>
        <div class="qr-code-container">
            <img src="../assets/images/QR-code.png" alt="QR code" class="qr-code" />
            <p>Zeskanuj ten kod QR, aby przejść na stronę FungEye na telefonie.</p>
        </div>
        <div class="os-buttons">
            <button class="btn fungeye-default-button os-button" @click="showInstructions('android')">
                <font-awesome-icon icon="fa-brands fa-android" class="button-icon" />
                <p class="os-name">Android</p>
            </button>
            <button class="btn fungeye-default-button os-button" @click="showInstructions('ios')">
                <font-awesome-icon icon="fa-brands fa-apple" class="button-icon" />
                <p class="os-name">iOS</p>
            </button>
        </div>
        <div class="android-instructions">
            <div v-if="showAndroidInstructions">
                <h2 class="page-subtitle">Instrukcja instalacji na systemie Android</h2>
                <p>
                    Aby zainstalować aplikację na urządzeniu z systemem Android, wykonaj następujące kroki:
                </p>
                <ol>
                    <li>Otwórz przeglądarkę internetową na swoim urządzeniu (np. Chrome). </li>
                    <img src="../assets/images/android/1.jpg" alt="Step 1" class="instruction-image" />
                    <li>Wprowadź adres
                        <a href="https://fungeye-app.westeurope.azurecontainer.io/" class="external-link">
                            <strong>https://fungeye-app.westeurope.azurecontainer.io/</strong>
                        </a>
                        w pasku
                        adresu.
                    </li>
                    <li>Po załadowaniu strony powinien pojawić się przycisk <strong>Zainstaluj</strong>. Kliknij go.
                    </li>
                    <img src="../assets/images/android/2.1.jpg" alt="Step 2" class="instruction-image" />
                    <p>Jeżeli nie widzisz tego przycisku, przejdź do kolejnych kroków.</p>
                    <li>Kliknij 3 pionowe kropki (
                        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                        ) na pasku narzędzi.
                    </li>
                    <img src="../assets/images/android/2.jpg" alt="Step 2" class="instruction-image" />
                    <li>Wybierz opcję <strong>Dodaj do ekranu głównego</strong>.</li>
                    <img src="../assets/images/android/3.jpg" alt="Step 3" class="instruction-image" />
                    <li>Następnie kliknij przycisk <strong>Zainstaluj</strong>.</li>
                    <img src="../assets/images/android/4.jpg" alt="Step 4" class="instruction-image" />
                    <li>Aplikacja pojawi się w panelu aplikacji. </li>
                    <img src="../assets/images/android/5.jpg" alt="Step 5" class="instruction-image" />
                    <li>Aby dodać aplikację do ekranu głównego, naciśnij i przytrzymaj ikonę aplikacji. Następnie
                        kliknij opcję <strong>Dodaj do ekranu startowego</strong>.</li>
                    <img src="../assets/images/android/6.jpg" alt="Step 6" class="instruction-image" />
                    <li>Aplikacja pojawi się na ekranie głównym.</li>
                    <img src="../assets/images/android/7.jpg" alt="Step 7" class="instruction-image" />
                </ol>
            </div>
        </div>
        <div class="ios-instructions">
            <div v-if="showIosInstructions">
                <h2 class="page-subtitle">Instrukcja instalacji na systemie iOS</h2>
                <p>
                    Aby zainstalować aplikację na urządzeniu z systemem iOS, wykonaj następujące kroki:
                </p>
                <ol>
                    <li>Otwórz przeglądarkę internetową na swoim urządzeniu (np. Safari, Chrome).</li>
                    <img src="../assets/images/ios/1.jpg" alt="Step 1" class="instruction-image" />
                    <li>Wprowadź adres
                        <a href="https://fungeye-app.westeurope.azurecontainer.io/" class="external-link">
                            <strong>https://fungeye-app.westeurope.azurecontainer.io/</strong>
                        </a> w pasku
                        adresu.</li>
                    <li>Kliknij przycisk <strong>Udostępnij</strong> oznaczony ikoną
                        <font-awesome-icon icon="fa-solid fa-arrow-up-from-bracket" /> (na dole ekranu w Safari, w
                        Chrome w prawym górnym rogu).
                    </li>
                    <img src="../assets/images/ios/2.jpg" alt="Step 1" class="instruction-image" />
                    <li>Na pasku narzędzi kliknij przycisk <strong>Do ekranu głównego</strong>.</li>
                    <img src="../assets/images/ios/3.jpg" alt="Step 2" class="instruction-image" />
                    <li>Wprowadź nazwę aplikacji i kliknij <strong>Dodaj</strong>.</li>
                    <img src="../assets/images/ios/4.jpg" alt="Step 3" class="instruction-image" />
                    <li>Aplikacja pojawi się na ekranie głównym.</li>
                    <img src="../assets/images/ios/5.jpg" alt="Step 4" class="instruction-image" />
                </ol>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'InstallationGuide',
    data() {
        return {
            showAndroidInstructions: true,
            showIosInstructions: false,
        }
    },
    methods: {
        showInstructions(os) {
            this.showAndroidInstructions = false;
            this.showIosInstructions = false;
            if (os === 'android') {
                this.showAndroidInstructions = true;
            } else if (os === 'ios') {
                this.showIosInstructions = true;
            }
        },
    }
}
</script>

<style scoped>
.container-md {
    justify-content: center;
    flex-wrap: wrap;
}

.qr-code-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    margin: 20px 0;
}

.qr-code {
    width: 200px;
    height: 200px;
    border: 3px solid var(--green);
    border-radius: 15px;
}

.os-buttons {
    display: flex;
    justify-content: center;
    align-items: baseline;
    gap: 20px;
}

.os-icon {
    width: auto;
    height: 25px;
}

.os-name {
    margin: 0;
    font-size: 1.2rem;
}

.android-instructions {
    margin: 20px 10px;
}

.external-link {
    color: var(--green);
}

ol li {
    font-size: 1.2em;
}

.instruction-image {
    max-width: 400px;
    width: 100%;
    margin: 20px 0;
}

@media screen and (max-width: 768px) {
    .page-title {
        font-size: 1.8rem;
    }

    .page-subtitle  {
        font-size: 1.5rem;
    }

    .external-link {
        font-size: 1rem;
    }
    
}

@media screen and (max-width: 390px) {
    .page-title {
        font-size: 1.5rem;
    }

    .external-link {
        font-size: 0.8rem;
    }

    .page-subtitle {
        font-size: 1.2rem;
    }

    .qr-code-container {
        display: none;
    }

    .os-buttons {
        flex-direction: column;
        justify-content: center;
        align-items: center;
        gap: 10px;
    }

    .instruction-image {
        max-width: 100%;
    }

}
</style>