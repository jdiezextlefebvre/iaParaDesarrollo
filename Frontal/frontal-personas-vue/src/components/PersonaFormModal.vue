<template>
  <dialog ref="dlg">
    <Form :validation-schema="schema" @submit="onSubmit" v-slot="{ errors }">
      <h3>{{ isEdit ? 'Editar Persona' : 'Nueva Persona' }}</h3>
      <div>
        <label for="nombre">Nombre</label>
        <Field id="nombre" name="nombre" as="input" />
        <span class="error">{{ errors.nombre }}</span>
      </div>
      <div>
        <label for="dni">DNI</label>
        <Field id="dni" name="dni" as="input" />
        <span class="error">{{ errors.dni }}</span>
      </div>
      <div>
        <label for="email">Email</label>
        <Field id="email" name="email" type="email" as="input" />
        <span class="error">{{ errors.email }}</span>
      </div>
      <div>
        <label for="edad">Edad</label>
        <Field id="edad" name="edad" type="number" as="input" />
        <span class="error">{{ errors.edad }}</span>
      </div>
      <footer>
        <button type="submit">{{ isEdit ? 'Guardar' : 'Crear' }}</button>
        <button type="button" @click="close">Cancelar</button>
      </footer>
    </Form>
  </dialog>
</template>
<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { Form, Field } from 'vee-validate';
import * as yup from 'yup';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';
import type { Persona } from '@/models/frontal/persona.model';

const schema = yup.object({
  nombre: yup.string().required('Nombre requerido'),
  dni: yup.string().required('DNI requerido'),
  email: yup.string().email('Formato email inválido').required('Email requerido'),
  edad: yup.number().min(0).max(130).required('Edad requerida')
});

interface Props { persona?: Persona | null }
const props = defineProps<Props>();
const emit = defineEmits<(e:'save', data: NuevaPersona | Partial<Persona>) => void>();
const dlg = ref<HTMLDialogElement | null>(null);
const isEdit = computed(() => !!props.persona);

function open() { dlg.value?.showModal(); }
function close() { dlg.value?.close(); }

function onSubmit(values: any) {
  emit('save', { ...values });
  close();
}

watch(() => props.persona, (val) => {
  if (val) open();
});

</script>
<style scoped>
.error { color:#b00020; font-size:.75rem; }
form { display:flex; flex-direction:column; gap:.75rem; min-width:280px; }
label { display:block; font-weight:600; }
input { width:100%; }
footer { display:flex; gap:.5rem; justify-content:flex-end; }
</style>
