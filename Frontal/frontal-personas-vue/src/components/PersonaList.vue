<template>
  <div>
    <ul v-if="personas.length">
      <PersonaListItem
        v-for="p in personas"
        :key="p.id"
        :persona="p"
        @edit="id => $emit('edit', id)"
        @delete="id => $emit('delete', id)"
        @detail="id => $emit('detail', id)"
      />
    </ul>
    <p v-else>No hay personas.</p>
  </div>
</template>
<script setup lang="ts">
import type { Persona } from '@/models/frontal/persona.model';
import PersonaListItem from './PersonaListItem.vue';

interface Props { personas: Persona[] }
const props = defineProps<Props>();
</script>
