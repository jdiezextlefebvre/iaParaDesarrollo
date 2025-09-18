<template>
  <dialog ref="dlg" class="personas-modal" @click="onBackdrop">
    <Form :validation-schema="schema" @submit="onSubmit">
      <h3>Nueva Persona</h3>
      <div class="field">
        <label for="nombre">Nombre</label>
        <Field id="nombre" name="nombre" as="input" />
        <ErrorMessage name="nombre" class="error" />
      </div>
      <div class="field">
        <label for="dni">DNI</label>
        <Field id="dni" name="dni" as="input" />
        <ErrorMessage name="dni" class="error" />
      </div>
      <div class="field">
        <label for="email">Email</label>
        <Field id="email" name="email" type="email" as="input" />
        <ErrorMessage name="email" class="error" />
      </div>
      <div class="field">
        <label for="edad">Edad</label>
        <Field id="edad" name="edad" type="number" as="input" />
        <ErrorMessage name="edad" class="error" />
      </div>
      <footer class="actions">
        <button type="submit">Crear</button>
        <button type="button" @click="close">Cancelar</button>
      </footer>
    </Form>
  </dialog>
</template>
<script setup lang="ts">
import { ref, watch, defineEmits, defineProps } from 'vue';
import { Form, Field, ErrorMessage } from 'vee-validate';
import * as yup from 'yup';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';

const visible = ref(false);
const dlg = ref<HTMLDialogElement | null>(null);

const emit = defineEmits<(e:'create', data: NuevaPersona)=>void>();
const props = defineProps<{ open: boolean }>();

const schema = yup.object({
  nombre: yup.string().required('Nombre requerido'),
  dni: yup.string().required('DNI requerido'),
  email: yup.string().email('Email inválido').required('Email requerido'),
  edad: yup.number().min(0).max(130).required('Edad requerida')
});

function onSubmit(values: any) {
  emit('create', values as NuevaPersona);
  close();
}

function open() { if (!visible.value) { visible.value = true; dlg.value?.showModal(); } }
function close() { if (visible.value) { visible.value = false; dlg.value?.close(); } }
function onBackdrop(e: MouseEvent) { if (e.target === dlg.value) close(); }

watch(() => props.open, (val) => { if (val) open(); else close(); }, { immediate: true });
</script>
<style scoped>
.personas-modal::backdrop { background: rgba(0,0,0,.5); }
.field { display:flex; flex-direction:column; gap:.25rem; margin-bottom:1rem; }
label { font-weight:600; }
input { padding:.6rem .7rem; border:1px solid #ced4da; border-radius:6px; font-size:.9rem; }
input:focus { outline:none; border-color:#337ab7; box-shadow:0 0 0 2px rgba(51,122,183,.15); }
.error { color:#dc3545; font-size:.7rem; }
.actions { margin-top:1rem; display:flex; gap:.5rem; justify-content:flex-end; }
button { cursor:pointer; padding:.55rem 1rem; border-radius:6px; border:1px solid #337ab7; background:#337ab7; color:#fff; }
button[type="button"] { background:#6c757d; border-color:#6c757d; }
button:disabled { opacity:.6; cursor:not-allowed; }
</style>
