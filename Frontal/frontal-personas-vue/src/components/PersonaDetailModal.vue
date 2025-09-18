<template>
  <dialog ref="dlg" class="personas-modal" @keydown.esc.prevent="close" @click="onBackdrop">
    <div v-if="persona" class="detail-wrapper">
      <header class="modal-header">
        <h3>{{ persona.nombre }}</h3>
        <div class="acciones">
          <button class="icon edit" @click="emit('edit', persona.id)" title="Editar">✏️</button>
          <button class="icon delete" @click="emit('delete', persona.id)" title="Borrar">🗑️</button>
          <button class="icon close" @click="close" title="Cerrar">✖</button>
        </div>
      </header>
      <div class="modal-body">
        <div class="campo"><strong>Nombre:</strong><span>{{ persona.nombre }}</span></div>
        <div class="campo"><strong>DNI:</strong><span>{{ persona.dni }}</span></div>
        <div class="campo"><strong>Email:</strong><span>{{ persona.email }}</span></div>
        <div class="campo"><strong>Edad:</strong><span>{{ persona.edad }} años</span></div>
      </div>
      <footer class="modal-footer">
        <button @click="close">Cerrar</button>
      </footer>
    </div>
  </dialog>
</template>
<script setup lang="ts">
import { ref, watch, onMounted } from 'vue';
import type { Persona } from '@/models/frontal/persona.model';

interface Props { persona: Persona | null }
const props = defineProps<Props>();
const emit = defineEmits<(e:'edit'|'delete', id:string)=>void>();
const dlg = ref<HTMLDialogElement | null>(null);

function close() { dlg.value?.close(); }
function open() { dlg.value?.showModal(); }
function onBackdrop(e: MouseEvent) { if (e.target === dlg.value) close(); }

watch(() => props.persona, (val) => { if (val) open(); else close(); });
onMounted(() => { if (props.persona) open(); });
</script>
<style scoped>
.personas-modal::backdrop { background: rgba(0,0,0,.5); }
dialog { border: none; padding:0; border-radius:12px; }
.detail-wrapper { min-width:300px; background:#fff; border-radius:12px; box-shadow:0 4px 18px rgba(0,0,0,.15); overflow:hidden; }
.modal-header { display:flex; align-items:center; justify-content:space-between; padding:1rem 1.25rem; background:#f8f9fa; border-bottom:1px solid #e5e7eb; }
.acciones { display:flex; gap:.4rem; }
.icon { background:transparent; border:none; cursor:pointer; font-size:.9rem; padding:.25rem .4rem; }
.icon.edit { color:#0d6efd; }
.icon.delete { color:#dc3545; }
.icon.close { color:#6c757d; }
.modal-body { padding:1rem 1.25rem; display:flex; flex-direction:column; gap:.6rem; }
.campo { display:flex; gap:.5rem; font-size:.9rem; }
.campo strong { min-width:70px; font-weight:600; }
.modal-footer { padding:.75rem 1.25rem; background:#f8f9fa; border-top:1px solid #e5e7eb; text-align:right; }
button { border:1px solid #337ab7; background:#337ab7; color:#fff; border-radius:6px; padding:.45rem .9rem; cursor:pointer; }
button:hover { background:#28679b; }
</style>
