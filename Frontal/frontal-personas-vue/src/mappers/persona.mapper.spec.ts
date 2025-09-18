import { fromPersonaRestDTOToPersona, fromPersonaRestDTOListToPersonaList, fromModificarPersonaToDatosModificarPersonaRestDTO, fromNuevaPersonaToDatosNuevaPersonaRestDTO } from './persona.mapper';

describe('persona.mapper', () => {
  it('map single dto', () => {
    const model = fromPersonaRestDTOToPersona({ id:'1', nombre:'A', dni:'X', email:'a@test.com', edad:30 });
    expect(model.nombre).toBe('A');
  });
  it('map list', () => {
    const list = fromPersonaRestDTOListToPersonaList([{ edad:20 } as any]);
    expect(list[0].id).toBe('');
  });
  it('new persona dto', () => {
    const dto = fromNuevaPersonaToDatosNuevaPersonaRestDTO({ nombre:'B', dni:'Y', email:'b@test.com', edad:25 });
    expect(dto.nombre).toBe('B');
  });
  it('modify persona dto only changed fields', () => {
    const dto = fromModificarPersonaToDatosModificarPersonaRestDTO({ email:'c@test.com' });
    expect(Object.keys(dto)).toEqual(['email']);
  });
});
