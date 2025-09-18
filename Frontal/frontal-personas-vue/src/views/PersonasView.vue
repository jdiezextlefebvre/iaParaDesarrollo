<template>
  <section>
    <header class="header">
      <h2>Personas</h2>
      <div class="actions">
        <button @click="openCreate">Nueva</button>
        <button @click="load" :disabled="loading">Recargar</button>
      </div>
    </header>
    <p v-if="error" class="error">{{ error }}</p>
    <p v-if="loading">Cargando...</p>
    <PersonaList
      v-if="!loading"
      :personas="personas"
      @edit="openEdit"
      @delete="remove"
      @detail="openDetail"
    />
  <PersonaCreateModal :open="createOpen" @create="create" />
  <PersonaEditModal :open="editOpen" :persona="personaEdit" @update="update" />
    <PersonaDetailModal :persona="personaDetail" />
  </section>
</template>

<script setup lang="ts">
import { onMounted, computed, ref } from 'vue';
import { usePersonasStore } from '@/stores/personas.store';
import PersonaList from '@/components/PersonaList.vue';
import PersonaCreateModal from '@/components/PersonaCreateModal.vue';
import PersonaEditModal from '@/components/PersonaEditModal.vue';
import PersonaDetailModal from '@/components/PersonaDetailModal.vue';
import type { Persona } from '@/models/frontal/persona.model';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';

const store = usePersonasStore();
const personas = computed(() => store.items);
const loading = computed(() => store.loading);
const error = computed(() => store.error);

const personaEdit = ref<Persona | null>(null);
const personaDetail = ref<Persona | null>(null);
const createOpen = ref(false);
const editOpen = ref(false);

function load() { store.fetchAll(); }
function openCreate() { createOpen.value = true; }
function openEdit(id: string) { const p = store.getById(id); if (p) { personaEdit.value = p; editOpen.value = true; } }
function openDetail(id: string) { const p = store.getById(id); if (p) personaDetail.value = p; }
async function remove(id: string) { if (confirm('¿Borrar?')) await store.remove(id); }
async function create(data: NuevaPersona) { await store.create(data); createOpen.value = false; await load(); }
async function update(id: string, data: Partial<Persona>) { await store.update(id, data); editOpen.value = false; await load(); }

onMounted(load);
</script>

<style scoped>
.error { color: red; }
.header { display:flex; justify-content:space-between; align-items:center; }
.actions { display:flex; gap:.5rem; }
</style>
