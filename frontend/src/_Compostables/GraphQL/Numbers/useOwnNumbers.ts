import { computed } from 'vue';
import type { INumber } from './INumber';
import { useQueryAll } from '@/_Compostables/GraphQL/Numbers/useQueryAll';

const useOwnNumbers = () => {
	
	const { all } = useQueryAll();
	
	return computed<INumber[]>(() => {
        return all.value.filter((e) => {
            return e.isOwned;
        });
    });
}

export { useOwnNumbers }