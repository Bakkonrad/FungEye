<template>
    <div class="modal-content">
        <h2>{{ showEditMushroomModal ? 'Edytuj grzyb' : 'Dodaj nowy grzyb' }}</h2>
        <BaseInput v-model="mushroomForm.polishName" placeholder="Nazwa polska grzyba" color="black" />
        <BaseInput v-model="mushroomForm.latinName" placeholder="Nazwa łacińska grzyba" color="black" />

        <!-- Dodaj zdjęcie grzyba -->
        <div class="photo-upload">
            <input style="display: none" type="file" accept="image/*" @change="onFileChange" ref="fileInput" multiple />
            <div class="drag-area" @click="$refs.fileInput.click()" @dragover.prevent="onDragOver"
                @dragleave.prevent="onDragLeave" @drop.prevent="onDrop">
                <span v-if="!isDragging">
                    <header v-if="!mushroomForm.image">Kliknij, aby wybrać zdjęcia grzyba</header>
                    <header v-else>Kliknij, aby wybrać inne zdjęcie grzyba</header>
                    <span class="select">lub przeciągnij je tutaj</span>
                </span>
                <span v-else>
                    <header>Upuść zdjęcia tutaj</header>
                </span>
            </div>
            <div class="container uploaded-images">
                <div v-for="(image, index) in mushroomForm.images" :key="index" class="image">
                    <span class="delete" @click="deleteImage(index)">&times;</span>
                    <img v-if="image" :src="image.url" alt="Zdjęcie grzyba" class="uploaded-image" />
                </div>
            </div>
        </div>

        <textarea v-model="mushroomForm.description" placeholder="Opis grzyba"
            class="edit-input form-control"></textarea>
        <div class="attribute-selection">
            <span v-for="attribute in availableAttributes" :key="attribute" @click="toggleAttributeFilter(attribute)"
                :class="['attribute', attributeClass(attribute), { 'active-attribute': isActiveAttribute(attribute) }]">
                {{ attribute }}
            </span>
        </div>
        <hr>
        <span class="buttons">
            <button class="btn fungeye-default-button" @click="saveChanges">{{ showEditMushroomModal ? 'Zapisz zmiany' :
                'Dodaj grzyb' }}</button>
            <button class="btn fungeye-red-button" @click="$emit('close')">Anuluj</button>
        </span>
    </div>
</template>

<script>
import BaseInput from "@/components/BaseInput.vue";
import FungiService from "@/services/FungiService";

export default {
    components: {
        BaseInput,
    },
    props: {
        showEditMushroomModal: Boolean,
        mushroomForm: Object,
        availableAttributes: Array,
    },
    data() {
        return {
            isDragging: false,
            selectedAttributes: [],
            files: [],
        };
    },
    methods: {
        onFileChange(e) {
            const files = Array.from(e.target.files);
            this.mushroomForm.images = files.map(file => ({
                id: file.name,
                url: URL.createObjectURL(file)
            }));
            this.files = files;
        },
        onDragOver(e) {
            e.preventDefault();
            this.isDragging = true;
        },
        onDragLeave(e) {
            e.preventDefault();
            this.isDragging = false;
        },
        onDrop(e) {
            e.preventDefault();
            this.isDragging = false;
            const files = Array.from(e.dataTransfer.files);
            this.mushroomForm.images = files.map(file => ({
                id: file.name,
                url: URL.createObjectURL(file)
            }));
            this.files = files;
        },
        deleteImage(index) {
            this.mushroomForm.images.splice(index, 1);
        },
        toggleAttributeFilter(attribute) {
            if (this.selectedAttributes.includes(attribute)) {
                this.selectedAttributes = this.selectedAttributes.filter((a) => a !== attribute);
            } else {
                // if edible is selected, remove inedible and poisonous (and vice versa)
                if (attribute === 'jadalny') {
                    this.selectedAttributes = this.selectedAttributes.filter((a) => a !== 'niejadalny' && a !== 'trujący');
                } else if (attribute === 'niejadalny' || attribute === 'trujący') {
                    this.selectedAttributes = this.selectedAttributes.filter((a) => a !== 'jadalny');
                }
                this.selectedAttributes.push(attribute);
            }
        },
        isActiveAttribute(attribute) {
            return this.selectedAttributes.includes(attribute);
        },
        attributeClass(attribute) {
            return {
                coniferous: attribute === 'iglaste',
                deciduous: attribute === 'liściaste',
                edible: attribute === 'jadalny',
                inedible: attribute === 'niejadalny',
                poisonous: attribute === 'trujący',
            };
        },
        saveChanges() {
            console.log(this.mushroomForm);
            console.log(this.selectedAttributes);
            if (this.mushroomForm.polishName === '' || this.mushroomForm.latinName === '' || this.mushroomForm.image === '' || this.mushroomForm.description === '' || this.selectedAttributes.length === 0) {
                alert('Wypełnij wszystkie pola');
                return;
            }
            const fungi = {
                polishName: this.mushroomForm.polishName,
                latinName: this.mushroomForm.latinName,
                description: this.mushroomForm.description,
                attributes: this.selectedAttributes,
            };
            const images = this.files;
            if (this.showEditMushroomModal) {
                this.editMushroom(fungi, images);
            } else {
                this.addMushroom(fungi, images);
            }
        },
        editMushroom(fungi, images) {
            const response = FungiService.editFungi(fungi, images);
            if (response.success === false) {
                alert('Nie udało się edytować grzyba');
                return;
            }
            this.$emit('close');
        },
        addMushroom(fungi, images) {
            // console.log(images);
            // const response = {success: true};
            const response = FungiService.addFungi(fungi, images);
            if (response.success === false) {
                alert('Nie udało się dodać grzyba');
                return;
            }
            this.$emit('close');
        },
    }
}

</script>

<style scoped>
.modal-content {
    background-color: var(--beige);
    padding: 20px;
    border-radius: 10px;
    width: 450px;
}

.edit-input {
    color: black !important;
    margin-bottom: 10px;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

.photo-upload {
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    margin-bottom: 15px;
}

.drag-area {
    width: 100%;
    border: 2px dashed #ccc;
    background: rgba(255, 255, 255, 0.3) !important;
    color: rgba(0, 0, 0, 0.572) !important;
    border-radius: 10px;
    padding: 20px;
    text-align: center;
    cursor: pointer;
    transition: 0.4s;
}

.uploaded-images {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    gap: 10px;
    margin-top: 10px;
}

.uploaded-image {
    width: 100px;
    height: 100px;
    object-fit: cover;
    border-radius: 10px;
}

.container .image {
    position: relative;
    width: 100px;
    height: 100px;
    margin-right: 10px;
    margin-bottom: 10px;
    border-radius: 10px;
    overflow: hidden;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
}

.container .image .delete {
    z-index: 999;
    position: absolute;
    top: 0;
    right: 0;
    width: 20px;
    height: 20px;
    background-color: rgba(0, 0, 0, 0.5);
    color: white;
    font-size: 1.2em;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
}

.container .image .delete:hover {
    background-color: rgba(0, 0, 0, 0.8);
}

.mushroom-actions {
    position: absolute;
    top: 10px;
    right: 10px;
    display: flex;
    gap: 10px;
}

.attribute-filter {
    margin: 20px 0;
    text-align: center;
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    gap: 5px;
}

/* css dla filtrów atrybutów */
.attribute-filter span {
    margin: 0 7px;
    cursor: pointer;
    font-size: 17px;
    padding: 1px 10px;
    border-radius: 15px;
    /* background-color: #f0f0f0; */
    transition: font-weight 0.3s, background-color 0.3s;
    user-select: none;
}

.active-attribute {
    font-weight: bold;
}

.attribute-selection {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    gap: 10px;
    margin: 1rem 0
}

/* css dla atrybutów w kartach grzybów */
.attributes span {
    padding: 2px 15px;
    border-radius: 15px;
    font-weight: 300;
    transition: font-weight 0.3s;
    user-select: none;
}

/* pogrubienie dla aktywnych atrybutów */
.active-attribute.coniferous,
.attribute-filter span.coniferous.active-attribute,
.active-attribute.deciduous,
.attribute-filter span.deciduous.active-attribute,
.active-attribute.edible,
.attribute-filter span.edible-active-attribute,
.active-attribute.inedible,
.attribute-filter span.inedible.active-attribute,
.active-attribute.poisonous,
.attribute-filter span.poisonous.active-attribute {
    font-weight: bold;
}

.attributes {
    display: flex;
    margin-top: 5px;
    list-style-type: none;
    padding: 0;
    display: flex;
    flex-wrap: wrap;
    flex-direction: row;
    gap: 10px;
}

.buttons {
    display: flex;
    justify-content: space-between;
    gap: 10px;
}

.buttons button {
    width: 100%;
}
</style>