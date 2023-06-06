import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import isEmpty from '@/Utility/isEmpty';

const useMutationRemoveFromTo = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation removeFromTo($e164: String!) {
		messages {
			removeFromTo(e164: $e164) {
				status
			}
		}
	}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemove error', error);
	});
	
	const fn = (e164: string | null) => {
		
		if (e164 === null) {
			console.warn('e164 === null');
			return;
		}
		if (isEmpty(e164)) {
			console.warn('isEmpty(e164.e164)');
			return;
		}
		
		mutate({
			e164: e164,
		});
	};
	
	return {
		remove: fn,
		removeLoading: loading,
		removeError: onError,
		removeDone: onDone,
	}
}

export { useMutationRemoveFromTo }